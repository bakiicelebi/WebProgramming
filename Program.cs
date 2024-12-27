using Microsoft.EntityFrameworkCore;
using WebProject.Data.Seeders;
using WebProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Veritaban� ba�lant�s�n� yap�land�rma
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseSqlServer("Server=DESKTOP-EE4GU60\\SQLEXPRESS;Database=BarberDB;Trusted_Connection=True;TrustServerCertificate=True;");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Geli�tirme ortam�nda hata sayfas� g�sterimi
    app.UseDeveloperExceptionPage();
}
else
{
    // �retim ortam�nda hata y�netimi
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Seed i�lemini do�ru bir �ekilde �a��rmak i�in DbContext'i inject etmek gerekiyor
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    ServiceSeeder.Seed(serviceProvider, isDevelopment: app.Environment.IsDevelopment());
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Varsay�lan route ayarlar�
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

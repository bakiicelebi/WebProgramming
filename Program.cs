using Microsoft.EntityFrameworkCore;
using WebProject.Data.Seeders;
using WebProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturumun zaman aþýmý süresi
    options.Cookie.HttpOnly = true; // Güvenlik için sadece HTTP'den eriþim
    options.Cookie.IsEssential = true; // Cookie'nin zorunlu olduðunu belirt
});

// Veritabaný baðlantýsýný yapýlandýrma
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseSqlServer("Server=DESKTOP-EE4GU60\\SQLEXPRESS;Database=BarberDB;Trusted_Connection=True;TrustServerCertificate=True;");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Geliþtirme ortamýnda hata sayfasý gösterimi
    app.UseDeveloperExceptionPage();
}
else
{
    // Üretim ortamýnda hata yönetimi
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Seed iþlemini doðru bir þekilde çaðýrmak için DbContext'i inject etmek gerekiyor
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    ServiceSeeder.Seed(serviceProvider, isDevelopment: app.Environment.IsDevelopment());
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

// Varsayýlan route ayarlarý
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using WebProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace WebProject.Data.Seeders
{
    public static class ServiceSeeder
    {
        public static void Seed(IServiceProvider serviceProvider, bool isDevelopment)
        {
            using var context = new DataBaseContext(serviceProvider.GetRequiredService<DbContextOptions<DataBaseContext>>());

            // Önce verileri sil
            if (context.Salons.Any())
            {
                context.Salons.RemoveRange(context.Salons);
            }

            if (context.Services.Any())
            {
                context.Services.RemoveRange(context.Services);
            }

            context.SaveChanges(); // Silme işlemini kaydedin

            // Salonları ekleyin (Id'yi elle atamıyoruz)
            var mainSalon = new Salon
            {
                Name = "Main Salon",
                Phone = "123456789",
                LogoUrl = "https://example.com/logo.png",
                OpeningHours = "{ \"monday\": \"9 AM - 6 PM\", \"tuesday\": \"9 AM - 6 PM\" }"
            };
            var downtownSalon = new Salon
            {
                Name = "Downtown Salon",
                Phone = "987654321",
                LogoUrl = "https://example.com/logo2.png",
                OpeningHours = "{ \"monday\": \"9 AM - 7 PM\", \"tuesday\": \"9 AM - 7 PM\" }"
            };

            context.Salons.AddRange(mainSalon, downtownSalon);
            context.SaveChanges(); // Salonları kaydedin

            // SalonId'leri kaydedildikten sonra, servislere doğru SalonId'leri atayın
            context.Services.AddRange(
                new Service
                {
                    Name = "Haircut",
                    Price = 1500,
                    Duration = TimeSpan.FromMinutes(30),
                    SalonId = mainSalon.SalonId, // Main Salon
                    ImageUrl = "https://cdn1.intermiami.news/uploads/52/2024/09/GettyImages-2172029895-1140x760.jpg"
                },
                new Service
                {
                    Name = "Shave",
                    Price = 800,
                    Duration = TimeSpan.FromMinutes(20),
                    SalonId = mainSalon.SalonId, // Main Salon
                    ImageUrl = "https://planla.co/blog/content/images/size/w1200/2024/11/berber.jpg"
                },
                new Service
                {
                    Name = "Beard Trim",
                    Price = 800,
                    Duration = TimeSpan.FromMinutes(15),
                    SalonId = downtownSalon.SalonId,
                    ImageUrl = "https://res.cloudinary.com/conferences-and-exhibitions-pvt-ltd/image/upload/v1666266289/Hair%20and%20Beauty%20Trends/2022/October/Hair-Care-for-Men/shutterstock_568818361_gzogmu.jpg"
                },
                new Service
                {
                    Name = "Children's Haircut",
                    Price = 550,
                    Duration = TimeSpan.FromMinutes(25),
                    SalonId = downtownSalon.SalonId,
                    ImageUrl = "https://content.latest-hairstyles.com/wp-content/uploads/fade-hairstyle-for-boys.jpg"
                },
                new Service
                {
                    Name = "Blow Dry",
                    Price = 700,
                    Duration = TimeSpan.FromMinutes(30),
                    SalonId = mainSalon.SalonId,
                    ImageUrl = "https://i.ytimg.com/vi/jLastv3derQ/maxresdefault.jpg"
                },
                new Service
                {
                    Name = "Eyebrow Shaping",
                    Price = 200,
                    Duration = TimeSpan.FromMinutes(15),
                    SalonId = mainSalon.SalonId,
                    ImageUrl = "https://cdn.shopify.com/s/files/1/2656/9554/files/Untitled_design_2_1_1.png?v=1711137676"
                },
                new Service
                {
                    Name = "Facial Treatment",
                    Price = 1200,
                    Duration = TimeSpan.FromMinutes(45),
                    SalonId = mainSalon.SalonId,
                    ImageUrl = "https://blog.californiaskincaresupply.com/wp-content/uploads/2022/04/man-beard-get-facial-1024x684.jpg"
                },
                new Service
                {
                    Name = "Hair Mask",
                    Price = 800,
                    Duration = TimeSpan.FromMinutes(30),
                    SalonId = mainSalon.SalonId,
                    ImageUrl = "https://nathabit.in/_nat/images/Man_Nutrimask_1_a7e1fd98bc.jpg?format=auto&width=1920&quality=75"
                },
                new Service
                {
                    Name = "Shave + Haircut",
                    Price = 1300,
                    Duration = TimeSpan.FromMinutes(50),
                    SalonId = mainSalon.SalonId,
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQk3zPbTWCz8Uq2pEUYtxZ2VYT1wHvFzkHHPQ&s"
                }
            );

            context.SaveChanges(); // Servisleri kaydedin
        }
    }
}

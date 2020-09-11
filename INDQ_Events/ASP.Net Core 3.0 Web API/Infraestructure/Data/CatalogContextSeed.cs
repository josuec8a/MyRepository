using ASP.Net_Core_3._0_Web_API.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.Infraestructure.Data
{
    public class CatalogContextSeed
    {
        public static async Task SeedAsync(CatalogContext catalogContext,
            ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                // TODO: Only run this if using a real database
                // context.Database.Migrate();
                if (!await catalogContext.Events.AnyAsync())
                {
                    await catalogContext.Events.AddRangeAsync(
                        GetPreconfiguredCatalogEvents());

                    await catalogContext.SaveChangesAsync();
                }

                //if (!await catalogContext.Users.AnyAsync())
                //{
                //    await catalogContext.Users.AddRangeAsync(
                //        GetPreconfiguredCatalogUsers());

                //    await catalogContext.SaveChangesAsync();
                //}

            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<CatalogContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(catalogContext, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        static IEnumerable<Event> GetPreconfiguredCatalogEvents()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            return new List<Event>()
            {
                new Event()
                {
                    Title = "Concierto 1",
                    Description = "Huertas 2",
                    Date = DateTime.Now.AddDays(1),
                    Image = "concierto1.jpg",
                    Attendances = 10,
                    WillYouAttend = true,
                    Location = geometryFactory.CreatePoint(new Coordinate(-109.011180, 25.767446))
                    //Lat = 25.765685m, 
                    //Lng = -109.013080m,
                },
                new Event()
                {
                    Title = "Concierto 2",
                    Description = "Paseo de las aves",
                    Date = DateTime.Now.AddDays(2),
                    Image = "concierto2.jpg",
                    Attendances = 20,
                    WillYouAttend = true,
                    Location = geometryFactory.CreatePoint(new Coordinate(-109.011324, 25.769880))
                    //Lat = 25.767953m,
                    //Lng = -109.012115m,
                },
                new Event()
                {
                    Title = "Concierto 3",
                    Description = "Huertas 1",
                    Date = DateTime.Now.AddDays(3),
                    Image = "concierto3.jpg",
                    Attendances = 30,
                    WillYouAttend = true,
                    Location = geometryFactory.CreatePoint(new Coordinate(-109.007202, 25.767601))
                    //Lat = 25.768717m,
                    //Lng = -109.007897m,
                },
                new Event()
                {
                    Title = "Concierto 4",
                    Description = "Platino",
                    Date = DateTime.Now.AddDays(4),
                    Image = "concierto4.jpg",
                    Attendances = 40,
                    WillYouAttend = true,
                    Location = geometryFactory.CreatePoint(new Coordinate( -109.005500, 25.769314))
                    //Lat = 25.774301m,
                    //Lng = -109.010795m,
                },
                new Event()
                {
                    Title = "Concierto 5",
                    Description = "Praderas de villa",
                    Date = DateTime.Now.AddDays(5),
                    Image = "concierto5.jpg",
                    Attendances = 50,
                    WillYouAttend = true,
                    Location = geometryFactory.CreatePoint(new Coordinate(-109.004399, 25.764918))
                    //Lat = 25.780159m,
                    //Lng = -109.007081m,
                },
                new Event()
                {
                    Title = "Concierto 6",
                    Description = "Jardines de villa",
                    Date = DateTime.Now.AddDays(1),
                    Image = "concierto6.jpg",
                    Attendances = 60,
                    WillYouAttend = true,
                    Location = geometryFactory.CreatePoint(new Coordinate(-109.002546, 25.767601))
                    //Lat = 25.801428m,
                    //Lng = -109.014395m,
                },
                new Event()
                {
                    Title = "Concierto 7",
                    Description = "Naranjos",
                    Date = DateTime.Now.AddDays(7),
                    Image = "concierto7.jpg",
                    Attendances = 70,
                    WillYouAttend = true,
                    Location = geometryFactory.CreatePoint(new Coordinate(-109.002872, 25.771500))
                    //Lat = 25.802777m,
                    //Lng = -109.003482m,
                },
                new Event()
                {
                    Title = "Concierto 8",
                    Description = "San Fernando",
                    Date = DateTime.Now.AddDays(8),
                    Image = "concierto8.jpg",
                    Attendances = 80,
                    WillYouAttend = true,
                    Location = geometryFactory.CreatePoint(new Coordinate(-109.008529, 25.775039))
                    //Lat = 25.816740m,
                    //Lng = -108.990972m,
                },
                new Event()
                {
                    Title = "Concierto 9",
                    Description = "12 Octubre",
                    Date = DateTime.Now.AddDays(9),
                    Image = "concierto9.jpg",
                    Attendances = 90,
                    WillYouAttend = true,
                    Location = geometryFactory.CreatePoint(new Coordinate(-109.011332, 25.779074))
                    //Lat = 25.822935m,
                    //Lng = -108.971702m,
                },
                new Event()
                {
                    Title = "Concierto 10",
                    Description = "Privadas Country",
                    Date = DateTime.Now.AddDays(10),
                    Image = "concierto10.jpg",
                    Attendances = 100,
                    WillYouAttend = true,
                    Location = geometryFactory.CreatePoint(new Coordinate(-109.010006, 25.784236))
                    //Lat = 25.807181m,
                    //Lng = -108.956021m,
                },
                new Event()
                {
                    Title = "Concierto 11",
                    Description = "Jardines del Country",
                    Date = DateTime.Now.AddDays(11),
                    Image = "concierto11.jpg",
                    Attendances = 110,
                    WillYouAttend = true,
                    Location = geometryFactory.CreatePoint(new Coordinate(-109.010206, 25.797150))
                    //Lat = 25.758223m,
                    //Lng = -108.819938m,
                },
                new Event()
                {
                    Title = "Concierto 12",
                    Description = "San Francisco",
                    Date = DateTime.Now.AddDays(12),
                    Image = "concierto12.jpg",
                    Attendances = 120,
                    WillYouAttend = true,
                    Location = geometryFactory.CreatePoint(new Coordinate(-109.011758, 25.803641))
                    //Lat = 25.706756m,
                    //Lng = -108.716494m,
                },
                new Event()
                {
                    Title = "Concierto 13",
                    Description = "Residencial del Valle",
                    Date = DateTime.Now.AddDays(13),
                    Image = "concierto13.jpg",
                    Attendances = 130,
                    WillYouAttend = true,
                    Location = geometryFactory.CreatePoint(new Coordinate(-109.000969, 25.810424))
                    //Lat = 25.706756m,
                    //Lng = -108.716494m,
                }
            };
        }

        //static IEnumerable<User> GetPreconfiguredCatalogUsers()
        //{
        //    return new List<User>()
        //    {
        //        new User()
        //        {
        //            FirstName = "Josué",
        //            LastName = "Cedano",
        //            Email = "josue.cedano@outlook.com",
        //            Password = "12345",
        //            Gender = "M"
        //        }
        //    };
        //}
    }
}

namespace WebApiV2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApiV2.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationsDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApiV2.Models.ApplicationsDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            /*context.Pais.AddOrUpdate(x => x.Id,
                new Pais { Nombre = "España", Habitantes = 4600000},
                new Pais { Nombre = "ALemania", Habitantes = 5000000},
                new Pais { Nombre = "Francia", Habitantes = 6000000},
                new Pais { Nombre = "ITalia", Habitantes = 70000000}
                );*/
        }
    }
}

using System;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Test.EntityFramework
{
    public class AppDB: DbContext
    {
        public static string DBPath { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<Region> Regions { get; set; }
        private static Boolean _schemaCreated = false;
        private string _dbPath;

        public AppDB()
        {
            _schemaCreated = Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) 
        {
            options.UseSqlite($"Filename={DBPath}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<City>().HasKey(city => city.ID);
            builder.Entity<Region>().HasKey(region => region.ID);
        }

        public void ClearCities()
        {
            foreach (City city in this.Cities)
            {
                this.Remove<City>(city);
            }
        }
    }

    public class City : IComparable
    {
        public int ID { get; set; }
        public String Name { get; set; }

        public int CompareTo(object obj)
        {
            var result = 0;
            var another = obj as City;

            if (another != null)
            {
                result = string.Compare(this.Name, another.Name, StringComparison.Ordinal);
            }

            return (result);
        }
    }

    public class Region: IComparable
    {
        public int ID { get; set; }
        public String Name { get; set; }

        //private City city;

        public int CompareTo(object obj)
        {
            int result = 0;
            var another = obj as Region;

            if (another != null)
            {
                result = string.Compare(this.Name, another.Name, StringComparison.Ordinal);
            }

            return (result);
        }
    }
}

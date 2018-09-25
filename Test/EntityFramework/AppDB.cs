using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.EntityFramework
{
    public class AppDB: DbContext
    {
        public static string DBPath { get; set; }
        private static Boolean _schemaCreated = false;

        public DbSet<EFCity> Cities { get; set; }
        public DbSet<EFRegion> Regions { get; set; }

        public AppDB()
        {
            if (_schemaCreated == false)
            {
                _schemaCreated = Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlite($"Filename={DBPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public void ClearCities()
        {
            foreach (EFCity city in this.Cities)
            {
                this.Remove<EFCity>(city);
            }
        }

        public void ClearRegions()
        {
            foreach (EFRegion region in this.Regions)
            {
                this.Remove<EFRegion>(region);
            }
        }
    }

    public class EFCity : IComparable
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public int RegionID { get; set; }

        [ForeignKey("RegionID")]
        public EFRegion region;

        public int CompareTo(object obj)
        {
            var result = 0;
            var another = obj as EFCity;

            if (another != null)
            {
                result = string.Compare(this.Name, another.Name, StringComparison.Ordinal);
            }

            return (result);
        }

        public void LinkToRegion(EFRegion region)
        {
            region.AppendCity(this);
        }
    }

    public class EFRegion: IComparable
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public List<EFCity> cities;

        public int CompareTo(object obj)
        {
            int result = 0;
            var another = obj as EFRegion;

            if (another != null)
            {
                result = string.Compare(this.Name, another.Name, StringComparison.Ordinal);
            }

            return (result);
        }

        public void AppendCity(EFCity city)
        {
            if (this.cities == null) {

                this.cities = new List<EFCity>();
            }

            this.cities.Add(city);
            city.region = this;
        }
    }
}

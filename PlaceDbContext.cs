using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
//using System.Data.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Oracle.EntityFrameworkCore.Storage.Internal;
using UdemyReact.Model;


namespace UdemyReact
{
    public class PlaceDbContext : DbContext
    {
        public PlaceDbContext(DbContextOptions<PlaceDbContext> options)
           : base(options) 
        { 
        }

        [JsonPropertyName("places")]
        public DbSet<Place> Place { get; set; }

        [JsonPropertyName("images")]
        public DbSet<Image> Image { get; set; }

        [JsonPropertyName("userplaces")]
        public DbSet<UserPlace> UserPlace { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("DDV");
            //modelBuilder.Entity<Place>().Property("Image").IsRequired();
            //modelBuilder.Entity<Place>().HasOne(p => p.Image).WithOne().HasForeignKey<Image>(i => i.Id);
            //modelBuilder.Entity<Place>().HasOne(p => p.Image).WithOne().HasPrincipalKey<Image>(i => i.Id);
            //modelBuilder.Entity<Place>().ToTable("Place");
            //modelBuilder.Entity<Image>().ToTable("Place");
            base.OnModelCreating(modelBuilder);
        }
    }

}

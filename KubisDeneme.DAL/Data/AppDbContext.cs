using Microsoft.EntityFrameworkCore;
using KubisDeneme.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KubisDeneme.DAL.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Kitap> Kitaplar { get; set; }
        public DbSet<Yazar> Yazarlar { get; set; }
        public DbSet<Ulke> Ulkeler { get; set; }
        public DbSet<KitapTuru> KitapTurleri { get; set; }
        public DbSet<KitapYazar> KitapYazarlar { get; set; }
        public DbSet<KitapKitapTuru> KitapKitapTurleri { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kitap - Yazar ilişkisi (Many-to-Many)
            modelBuilder.Entity<KitapYazar>()
                .HasKey(ky => new { ky.KitapId, ky.YazarId });

            modelBuilder.Entity<KitapYazar>()
                .HasOne(ky => ky.Kitap)
                .WithMany(k => k.KitapYazarlar)
                .HasForeignKey(ky => ky.KitapId);

            modelBuilder.Entity<KitapYazar>()
                .HasOne(ky => ky.Yazar)
                .WithMany(y => y.KitapYazarlar)
                .HasForeignKey(ky => ky.YazarId);

            // Kitap - KitapTuru ilişkisi (Many-to-Many)
            modelBuilder.Entity<KitapKitapTuru>()
                .HasKey(kt => new { kt.KitapId, kt.KitapTuruId });

            modelBuilder.Entity<KitapKitapTuru>()
                .HasOne(kt => kt.Kitap)
                .WithMany(k => k.KitapKitapTurleri)
                .HasForeignKey(kt => kt.KitapId);

            modelBuilder.Entity<KitapKitapTuru>()
                .HasOne(kt => kt.KitapTuru)
                .WithMany(t => t.KitapKitapTurleri)
                .HasForeignKey(kt => kt.KitapTuruId);

            // Yazar - Ülke ilişkisi (One-to-Many) 
            modelBuilder.Entity<Yazar>()
                .HasOne(y => y.Ulke)
                .WithMany(u => u.Yazarlar)
                .HasForeignKey(y => y.UlkeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

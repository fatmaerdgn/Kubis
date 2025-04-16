using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KubisDeneme.DAL.Models;

namespace KubisDeneme.DAL.Configurations
{
    public class KitapYazarConfigurations : IEntityTypeConfiguration<KitapYazar>
    {
        public void Configure(EntityTypeBuilder<KitapYazar> builder)
        {
            builder.HasKey(ky => new { ky.KitapId, ky.YazarId });

            builder.HasOne(ky => ky.Kitap)
                .WithMany(k => k.KitapYazarlar)
                .HasForeignKey(ky => ky.KitapId);

            builder.HasOne(ky => ky.Yazar)
                .WithMany(y => y.KitapYazarlar)
                .HasForeignKey(ky => ky.YazarId);
        }
    }
}

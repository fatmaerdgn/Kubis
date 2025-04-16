using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KubisDeneme.DAL.Models;

namespace KubisDeneme.DAL.Configurations
{
    public class KitapKitapTuruConfigurations : IEntityTypeConfiguration<KitapKitapTuru>
    {
        public void Configure(EntityTypeBuilder<KitapKitapTuru> builder)
        {
            builder.HasKey(kkt => new { kkt.KitapId, kkt.KitapTuruId });

            builder.HasOne(kkt => kkt.Kitap)
                .WithMany(k => k.KitapKitapTurleri)
                .HasForeignKey(kkt => kkt.KitapId);

            builder.HasOne(kkt => kkt.KitapTuru)
                .WithMany(kt => kt.KitapKitapTurleri)
                .HasForeignKey(kkt => kkt.KitapTuruId);
        }
    }
}

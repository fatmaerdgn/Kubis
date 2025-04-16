using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KubisDeneme.DAL.Models;

namespace KubisDeneme.DAL.Configurations
{
    public class YazarUlkeConfigurations : IEntityTypeConfiguration<Yazar>
    {
        public void Configure(EntityTypeBuilder<Yazar> builder)
        {
            // Ülke ilişkisi (One-to-Many)
            builder.HasOne(y => y.Ulke)
                .WithMany(u => u.Yazarlar)
                .HasForeignKey(y => y.UlkeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

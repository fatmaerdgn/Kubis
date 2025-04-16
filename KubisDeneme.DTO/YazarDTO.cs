using System.ComponentModel.DataAnnotations;

namespace KubisDeneme.DTO
{
    public class YazarDTO
    {
        public int? Id { get; set; }
        public string? Ad { get; set; }
        public string? ISNI { get; set; }
        public DateTime? DogumTarihi { get; set; } = DateTime.Now;
        public DateTime? EklenmeTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellenmeTarihi { get; set; } = DateTime.Now;

        //foreign key
        public int?UlkeId { get; set; }
        public UlkeDTO? Ulke { get; set; }

        //İlişkiler
        public List<KitapYazarDTO>? KitapYazarlar { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace KubisDeneme.DTO
{
    public class KitapTuruDTO
    {
        public int? Id { get; set; }
        public string? Ad { get; set; }
        public DateTime? EklenmeTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellenmeTarihi { get; set; } = DateTime.Now;

        //İlişkiler
        public List<KitapKitapTuruDTO>? KitapKitapTurleri { get; set; }
    }
}

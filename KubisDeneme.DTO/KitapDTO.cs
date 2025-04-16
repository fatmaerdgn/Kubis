using System.ComponentModel.DataAnnotations;

namespace KubisDeneme.DTO
{
    public class KitapDTO
    {
        public int? Id { get; set; }
        public string? KitapAdi { get; set; }
        public int? SayfaSayisi { get; set; }
        public int? İlkYayinYili { get; set; }
        public string? ISBN { get; set; }
        public DateTime? EklenmeTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellenmeTarihi { get; set; } = DateTime.Now;

        // İlişkiler
        public List<KitapYazarDTO>? KitapYazarlar { get; set; } = new List<KitapYazarDTO>();
        public List<KitapKitapTuruDTO>? KitapKitapTurleri { get; set; } = new List<KitapKitapTuruDTO>();
    }
}

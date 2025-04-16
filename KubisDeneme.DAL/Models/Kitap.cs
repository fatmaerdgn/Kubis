namespace KubisDeneme.DAL.Models
{
    public class Kitap
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public int SayfaSayisi { get; set; }
        public int İlkYayinYili { get; set; }
        public string ISBN { get; set; }
        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;

        public DateTime GuncellenmeTarihi { get; set; } = DateTime.Now;

        public bool AktifMi { get; set; } = true;

        // İlişkiler
        public List<KitapYazar> KitapYazarlar { get; set; }
        public List<KitapKitapTuru> KitapKitapTurleri { get; set; }
    }
}

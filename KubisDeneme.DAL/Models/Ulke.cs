namespace KubisDeneme.DAL.Models
{
    public class Ulke
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;
        public DateTime GuncellenmeTarihi { get; set; } = DateTime.Now;
        public bool AktifMi { get; set; } = true;
        // İlişki: Bir ülkede birden fazla yazar doğabilir
        public List<Yazar> Yazarlar { get; set; }
    }
}

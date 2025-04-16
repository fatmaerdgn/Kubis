namespace KubisDeneme.DAL.Models
{
    public class Yazar
    { 
        public int Id { get; set; }
        public string Ad { get; set; }
        public string ISNI { get; set; }
        public DateTime? DogumTarihi { get; set; }
        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;
        public DateTime GuncellenmeTarihi { get; set; } = DateTime.Now;
        public bool AktifMi { get; set; } = true;

        // İlişki: Bir yazar birden fazla kitap yazabilir
        public List<KitapYazar> KitapYazarlar { get; set; }

        //foreign key
        public int UlkeId { get; set; }
        public Ulke Ulke { get; set; }
    }
}

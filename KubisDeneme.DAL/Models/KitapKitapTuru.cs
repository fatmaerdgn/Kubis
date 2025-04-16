namespace KubisDeneme.DAL.Models
{
    public class KitapKitapTuru
    {
        public int KitapId { get; set; }
        public Kitap Kitap { get; set; }

        public int KitapTuruId { get; set; }
        public KitapTuru KitapTuru { get; set; }
    }
}

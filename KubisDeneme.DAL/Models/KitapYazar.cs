namespace KubisDeneme.DAL.Models
{
    public class KitapYazar
    {
        public int KitapId { get; set; }
        public Kitap Kitap { get; set; }

        public int YazarId { get; set; }
        public Yazar Yazar { get; set; }
    }
}

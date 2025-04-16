namespace KubisDeneme.DTO
{
    public class KitapYazarDTO
    {
        public int? KitapId { get; set; }
        public KitapDTO? Kitap { get; set; }
        public int? YazarId { get; set; }
        public YazarDTO? Yazar { get; set; }
    }
}
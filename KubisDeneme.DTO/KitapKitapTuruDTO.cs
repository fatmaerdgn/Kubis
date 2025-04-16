namespace KubisDeneme.DTO
{
    public class KitapKitapTuruDTO
    {
        public int? KitapId { get; set; }
        public KitapDTO? Kitap { get; set; }
        public int? KitapTuruId { get; set; }
        public KitapTuruDTO? KitapTuru { get; set; }
    }
}

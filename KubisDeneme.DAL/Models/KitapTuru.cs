﻿namespace KubisDeneme.DAL.Models
{
    public class KitapTuru
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;
        public DateTime GuncellenmeTarihi { get; set; } = DateTime.Now;
        public bool AktifMi { get; set; } = true;

        // İlişki: Bir kitap türü birden fazla kitapta kullanılabilir
        public List<KitapKitapTuru> KitapKitapTurleri { get; set; }
    }
}

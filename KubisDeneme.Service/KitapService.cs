using KubisDeneme.DAL.Data;
using KubisDeneme.DAL.Models;
using KubisDeneme.DTO;
using Microsoft.EntityFrameworkCore;

namespace KubisDeneme.Service
{
    public class KitapService : IKitapService
    {
        private readonly AppDbContext _context;
        public KitapService(AppDbContext context)
        {
            _context = context;
        }
        public List<KitapDTO> TumKitaplariGetir()
        {
            var kitaplar = _context.Kitaplar
                .Include(k => k.KitapYazarlar)
                    .ThenInclude(ky => ky.Yazar)
                .Include(k => k.KitapKitapTurleri)
                    .ThenInclude(kkt => kkt.KitapTuru)
                .Select(kitap => new KitapDTO
                {
                    Id = kitap.Id,
                    KitapAdi = kitap.Ad,
                    SayfaSayisi = kitap.SayfaSayisi,
                    İlkYayinYili = kitap.İlkYayinYili,
                    ISBN = kitap.ISBN,
                    EklenmeTarihi = kitap.EklenmeTarihi,
                    GuncellenmeTarihi = kitap.GuncellenmeTarihi,
                    //AktifMi = kitap.AktifMi,

                    // İlişkili Yazarlar
                    KitapYazarlar = kitap.KitapYazarlar.Select(ky => new KitapYazarDTO
                    {
                        KitapId = ky.KitapId,
                        //Kitap = new KitapDTO { Id = ky.Kitap.Id, KitapAdi = ky.Kitap.Ad },
                        Kitap = ky.Kitap != null ? new KitapDTO { Id = ky.Kitap.Id, KitapAdi = ky.Kitap.Ad } : null,
                        YazarId = ky.YazarId,
                        //Yazar = new YazarDTO { Id = ky.Yazar.Id, Ad = ky.Yazar.Ad }
                        Yazar = ky.Yazar != null ? new YazarDTO { Id = ky.Yazar.Id, Ad = ky.Yazar.Ad } : null,

                    }).ToList(),
                    // İlişkili Kitap Türleri
                    KitapKitapTurleri = kitap.KitapKitapTurleri
                        .Select(kkt => new KitapKitapTuruDTO
                        {
                            KitapId = kkt.KitapId,
                            Kitap = new KitapDTO { Id = kkt.Kitap.Id, KitapAdi = kkt.Kitap.Ad },
                            KitapTuruId = kkt.KitapTuruId,
                            KitapTuru = new KitapTuruDTO { Id = kkt.KitapTuru.Id, Ad = kkt.KitapTuru.Ad }
                        }).ToList()
                })
                .ToList();

            return kitaplar;
        }
        public KitapDTO? KitapGetir(int id)
        {
            var kitap = _context.Kitaplar
                .Include(k => k.KitapYazarlar)
                    .ThenInclude(ky => ky.Yazar)
                .Include(k => k.KitapKitapTurleri)
                    .ThenInclude(kkt => kkt.KitapTuru)
                .FirstOrDefault(k => k.Id == id);
            if (kitap == null)
            {
                return null;
            }

            return new KitapDTO
            {
                Id = kitap.Id,
                KitapAdi = kitap.Ad,
                SayfaSayisi = kitap.SayfaSayisi,
                İlkYayinYili = kitap.İlkYayinYili,
                ISBN = kitap.ISBN,
                EklenmeTarihi = kitap.EklenmeTarihi,
                GuncellenmeTarihi = kitap.GuncellenmeTarihi,
                //AktifMi = kitap.AktifMi,

                // İlişkili Yazarlar
                KitapYazarlar = kitap.KitapYazarlar.Select(ky => new KitapYazarDTO
                {
                    KitapId = ky.KitapId,
                    Kitap = ky.Kitap != null ? new KitapDTO { Id = ky.Kitap.Id, KitapAdi = ky.Kitap.Ad } : null,
                    YazarId = ky.YazarId,
                    Yazar = ky.Yazar != null ? new YazarDTO { Id = ky.Yazar.Id, Ad = ky.Yazar.Ad } : null
                }).ToList(),
                // İlişkili Kitap Türleri
                KitapKitapTurleri = kitap.KitapKitapTurleri
                    .Select(kkt => new KitapKitapTuruDTO
                    {
                        KitapId = kkt.KitapId,
                        Kitap = kkt.Kitap != null ? new KitapDTO { Id = kkt.Kitap.Id, KitapAdi = kkt.Kitap.Ad } : null,
                        KitapTuruId = kkt.KitapTuruId,
                        KitapTuru = kkt.KitapTuru != null ? new KitapTuruDTO { Id = kkt.KitapTuru.Id, Ad = kkt.KitapTuru.Ad } : null
                    }).ToList()
            };
        }        
        public void KitapEkle(KitapDTO? kitapDTO)
        {
            if (kitapDTO.EklenmeTarihi > DateTime.UtcNow)
            {
                throw new ArgumentException("Gelecek tarihli kayıt oluşturulamaz");
            }

            if (_context.Kitaplar.Any(x => x.ISBN == kitapDTO.ISBN))
            {
                throw new ArgumentException("Bu kitap ISBN mevcut.", nameof(kitapDTO.ISBN));
            }

            var guncellenmeTarihi = kitapDTO.GuncellenmeTarihi?.ToUniversalTime() ?? DateTime.UtcNow;
            var eklenmeTarihi = kitapDTO.EklenmeTarihi ?? DateTime.UtcNow;

            var kitap = new Kitap
            {
                Ad = kitapDTO.KitapAdi,
                SayfaSayisi = kitapDTO.SayfaSayisi ?? 0,
                İlkYayinYili = kitapDTO.İlkYayinYili ?? 0,
                ISBN = kitapDTO.ISBN,
                EklenmeTarihi = eklenmeTarihi,
                GuncellenmeTarihi = guncellenmeTarihi,
            };

            if (kitapDTO.EklenmeTarihi != null && kitapDTO.EklenmeTarihi != kitap.EklenmeTarihi)
            {
                kitap.EklenmeTarihi = kitapDTO.EklenmeTarihi.Value.ToUniversalTime();
            }

            _context.Kitaplar.Add(kitap);
            _context.SaveChanges(); // Kitap kaydedilip Id alınıyor.

            kitapDTO.Id = kitap.Id; // KitapDTO'ya ID set ediliyor.
            EkleKitapIliskileri(kitapDTO, kitap.Id);
        }
        private void EkleKitapIliskileri(KitapDTO kitapDTO, int kitapId)
        {
            try
            {
                if (kitapDTO.KitapYazarlar != null)
                {
                    foreach (var kyDTO in kitapDTO.KitapYazarlar)
                    {
                        if (kyDTO.YazarId == null)
                        {
                            throw new ArgumentNullException(nameof(kyDTO.YazarId), "YazarId boş olamaz.");
                        }
                        _context.KitapYazarlar.Add(new KitapYazar { KitapId = kitapId, YazarId = kyDTO.YazarId.Value });
                    }
                }

                if (kitapDTO.KitapKitapTurleri != null)
                {
                    foreach (var kktDTO in kitapDTO.KitapKitapTurleri)
                    {
                        if (kktDTO.KitapTuruId == null)
                        {
                            throw new ArgumentNullException(nameof(kktDTO.KitapTuruId), "KitapTuruId boş olamaz.");
                        }
                        _context.KitapKitapTurleri.Add(new KitapKitapTuru { KitapId = kitapId, KitapTuruId = kktDTO.KitapTuruId.Value });
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kitap ilişkileri eklenirken hata oluştu: {ex.Message}");
            }
        }
        public void KitapGuncelle(KitapDTO kitapDTO)
        {
            if (kitapDTO.EklenmeTarihi > DateTime.UtcNow)
            {
                throw new ArgumentException("Gelecek tarihli kayıt oluşturulamaz");
            }

            var kitap = _context.Kitaplar.Include(k => k.KitapYazarlar).Include(k => k.KitapKitapTurleri).FirstOrDefault(k => k.Id == kitapDTO.Id);

            if (_context.Kitaplar.Any(x => x.Id != kitapDTO.Id && x.ISBN == kitapDTO.ISBN))
            {
                throw new ArgumentException("Bu kitap ISBN mevcut.", nameof(kitapDTO.ISBN));
            }

            kitap.Ad = kitapDTO.KitapAdi;
            kitap.SayfaSayisi = kitapDTO.SayfaSayisi ?? 0;
            kitap.İlkYayinYili = kitapDTO.İlkYayinYili ?? 0;
            kitap.ISBN = kitapDTO.ISBN;
            kitap.GuncellenmeTarihi = kitapDTO.GuncellenmeTarihi?.ToUniversalTime() ?? DateTime.UtcNow;

            if (kitapDTO.EklenmeTarihi != null && kitapDTO.EklenmeTarihi != kitap.EklenmeTarihi)
            {
                kitap.EklenmeTarihi = kitapDTO.EklenmeTarihi.Value.ToUniversalTime();
            }

            // Kitap yazarları güncelle
            kitap.KitapYazarlar.Clear(); //Bellekteki ilişkileri temizle

            if (kitapDTO.KitapYazarlar != null)
            {
                foreach (var kyDTO in kitapDTO.KitapYazarlar)
                {
                    if (kyDTO.YazarId != null)
                    {
                        kitap.KitapYazarlar.Add(new KitapYazar { KitapId = kitap.Id, YazarId = kyDTO.YazarId.Value });
                    }
                }
            }

            // Kitap türleri güncelle
            kitap.KitapKitapTurleri.Clear(); //Bellekteki ilişkileri temizle

            if (kitapDTO.KitapKitapTurleri != null)
            {
                foreach (var kktDTO in kitapDTO.KitapKitapTurleri)
                {
                    if (kktDTO.KitapTuruId != null)
                    {
                        kitap.KitapKitapTurleri.Add(new KitapKitapTuru { KitapId = kitap.Id, KitapTuruId = kktDTO.KitapTuruId.Value });
                    }
                }
            }

            _context.SaveChanges();
        }
        public void KitapSil(int id)
        {
            var kitap = _context.Kitaplar.Find(id);
            if (kitap == null)
                return;
            _context.Kitaplar.Remove(kitap);
            _context.SaveChanges();
        }
    }
}

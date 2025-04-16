using KubisDeneme.DAL.Data;
using KubisDeneme.DAL.Models;
using KubisDeneme.DTO;
using Microsoft.EntityFrameworkCore;

namespace KubisDeneme.Service
{
    public class KitapTuruService : IKitapTuruService
    {
        private readonly AppDbContext _context;
        public KitapTuruService(AppDbContext context)
        {
            _context = context;
        }
        public List<KitapTuruDTO>? TumKitapTurleriniGetir()
        {
            var kitapTurleri = _context.KitapTurleri
                .Include(x => x.KitapKitapTurleri).ToList();
            return kitapTurleri.Select(x => new KitapTuruDTO
            {
                Id = x.Id,
                Ad = x.Ad,
                EklenmeTarihi = x.EklenmeTarihi,
                GuncellenmeTarihi = x.GuncellenmeTarihi,
                KitapKitapTurleri = x.KitapKitapTurleri.Select(y => new KitapKitapTuruDTO
                {
                    KitapId = y.KitapId,
                    KitapTuruId = y.KitapTuruId
                }).ToList()
            }).ToList();
        }
        public KitapTuruDTO? KitapTuruGetir(int id)
        {
            var kitapTuru = _context.KitapTurleri
                .Include(x => x.KitapKitapTurleri)
                .FirstOrDefault(x => x.Id == id);
            if (kitapTuru == null)
            {
                return null;
            }
            return new KitapTuruDTO
            {
                Id = kitapTuru.Id,
                Ad = kitapTuru.Ad,
                EklenmeTarihi = kitapTuru.EklenmeTarihi,
                GuncellenmeTarihi = kitapTuru.GuncellenmeTarihi,
                KitapKitapTurleri = kitapTuru.KitapKitapTurleri.Select(x => new KitapKitapTuruDTO
                {
                    KitapId = x.KitapId,
                    KitapTuruId = x.KitapTuruId
                }).ToList()
            };
        }
        public void KitapTuruEkle(KitapTuruDTO kitapTuruDTO)
        {
            if(_context.KitapTurleri.Any(x => x.Ad.ToLower() == kitapTuruDTO.Ad.ToLower()))
            {
                throw new ArgumentException("Bu kitap türü adı zaten mevcut.", nameof(kitapTuruDTO.Ad));
            }

            var eklenmeTarihi = kitapTuruDTO.EklenmeTarihi?.ToUniversalTime() ?? DateTime.UtcNow;
            var guncellenmeTarihi = kitapTuruDTO.GuncellenmeTarihi?.ToUniversalTime() ?? DateTime.UtcNow;

            var kitapTuru = new KitapTuru
            {
                Ad = kitapTuruDTO.Ad,
                EklenmeTarihi = eklenmeTarihi,
                GuncellenmeTarihi = guncellenmeTarihi
            };

            if (kitapTuruDTO.EklenmeTarihi != null && kitapTuruDTO.EklenmeTarihi != kitapTuru.EklenmeTarihi)
            {
                kitapTuru.EklenmeTarihi = kitapTuruDTO.EklenmeTarihi.Value.ToUniversalTime();
            }

            _context.KitapTurleri.Add(kitapTuru);
            _context.SaveChanges();
            kitapTuruDTO.Id = kitapTuru.Id;
        }
        public void KitapTuruGuncelle(KitapTuruDTO kitapTuruDTO)
        {
            // Veritabanına kaydetmeden önce son kontrol
            if (kitapTuruDTO.EklenmeTarihi > DateTime.UtcNow)
            {
                throw new ArgumentException("Gelecek tarihli kayıt oluşturulamaz");
            }

            var kitapTuru = _context.KitapTurleri.FirstOrDefault(x => x.Id == kitapTuruDTO.Id);

            // Veritabanında aynı isimde bir kitap türü olup olmadığını kontrol et
            if (_context.KitapTurleri.Any(x => x.Id != kitapTuruDTO.Id && x.Ad.ToLower() == kitapTuruDTO.Ad.ToLower()))
            {
                throw new ArgumentException("Bu kitap türü adı zaten mevcut.", nameof(kitapTuruDTO.Ad));
            }

            kitapTuru.Ad = kitapTuruDTO.Ad;   
            kitapTuru.GuncellenmeTarihi = kitapTuruDTO.GuncellenmeTarihi?.ToUniversalTime() ?? DateTime.UtcNow;

            if (kitapTuruDTO.EklenmeTarihi != null && kitapTuruDTO.EklenmeTarihi != kitapTuru.EklenmeTarihi)
            {
                kitapTuru.EklenmeTarihi = kitapTuruDTO.EklenmeTarihi.Value.ToUniversalTime();
            }

            _context.SaveChanges();           
        }
        public void KitapTuruSil(int id)
        {
            var kitapTuru = _context.KitapTurleri.FirstOrDefault(x => x.Id == id);
            if (kitapTuru == null)
            {
                return;
            }
            _context.KitapTurleri.Remove(kitapTuru);
            _context.SaveChanges();
        }
    }
}

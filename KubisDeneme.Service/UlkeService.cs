using KubisDeneme.DAL.Data;
using KubisDeneme.DAL.Models;
using KubisDeneme.DTO;
using Microsoft.EntityFrameworkCore;

namespace KubisDeneme.Service
{
    public class UlkeService : IUlkeService
    {
        private readonly AppDbContext _context;
        public UlkeService(AppDbContext context)
        {
            _context = context;
        }
        public List<UlkeDTO> TumUlkeleriGetir()
        {
            var ulkeler = _context.Ulkeler
                .Select(ulke => new UlkeDTO
                {
                    Id = ulke.Id,
                    Ad = ulke.Ad,
                    EklenmeTarihi = ulke.EklenmeTarihi,
                    GuncellenmeTarihi = ulke.GuncellenmeTarihi
                })
                .ToList();
            return ulkeler;
        }
        public UlkeDTO? UlkeGetir(int Id) 
        {
            var ulke = _context.Ulkeler.Find(Id);
            if (ulke == null)
            {
                return null; // Ülke bulunamazsa null döndür
            }
            return new UlkeDTO
            {
                Id = ulke.Id,
                Ad = ulke.Ad,
                EklenmeTarihi = ulke.EklenmeTarihi,
                GuncellenmeTarihi = ulke.GuncellenmeTarihi
            };
        }
        public void UlkeEkle(UlkeDTO ulke)
        {
            // Veritabanında aynı isimde bir ülke olup olmadığını kontrol et
            if (_context.Ulkeler.Any(u => u.Ad.ToLower() == ulke.Ad.ToLower()))
            {
                throw new ArgumentException("Bu ülke adı zaten mevcut.", nameof(ulke.Ad));
            }
            
            var guncellenmeTarihi = ulke.GuncellenmeTarihi?.ToUniversalTime() ?? DateTime.UtcNow;
            var eklenmeTarihi = ulke.EklenmeTarihi ?? DateTime.UtcNow;

            var entity = new Ulke
            {
                Ad = ulke.Ad,
                EklenmeTarihi = eklenmeTarihi,
                GuncellenmeTarihi = guncellenmeTarihi,
            };

            if (ulke.EklenmeTarihi != null && ulke.EklenmeTarihi != entity.EklenmeTarihi)
            {
                entity.EklenmeTarihi = ulke.EklenmeTarihi.Value.ToUniversalTime();
            }

            _context.Ulkeler.Add(entity);
            _context.SaveChanges();
            ulke.Id = entity.Id;
        }
        public void UlkeGuncelle(UlkeDTO ulke)
        {

            // Veritabanına kaydetmeden önce son kontrol
            if (ulke.EklenmeTarihi > DateTime.UtcNow)
            {
                throw new ArgumentException("Gelecek tarihli kayıt oluşturulamaz");
            }

            var entity = _context.Ulkeler.Find(ulke.Id);

            // Veritabanında aynı isimde bir ülke olup olmadığını kontrol et
            if (_context.Ulkeler.Any(u => u.Id != ulke.Id && u.Ad.ToLower() == ulke.Ad.ToLower()))
            {
                throw new ArgumentException("Bu ülke adı zaten mevcut.", nameof(ulke.Ad));
            }

            entity.Ad = ulke.Ad;
            entity.GuncellenmeTarihi = ulke.GuncellenmeTarihi?.ToUniversalTime() ?? DateTime.UtcNow;

            if (ulke.EklenmeTarihi != null && ulke.EklenmeTarihi != entity.EklenmeTarihi)
            {
                entity.EklenmeTarihi = ulke.EklenmeTarihi.Value.ToUniversalTime();
            }
            _context.SaveChanges();
        }
        public void UlkeSil(int ulkeId)
        {
            var entity = _context.Ulkeler.Find(ulkeId);
            if (entity == null)
            {
                throw new ArgumentException("Belirtilen ID'ye sahip ülke bulunamadı.", nameof(ulkeId));
            }
            _context.Ulkeler.Remove(entity);
            _context.SaveChanges();
        }
    }
}
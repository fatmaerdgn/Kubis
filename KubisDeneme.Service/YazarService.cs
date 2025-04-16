using KubisDeneme.DAL.Data;
using KubisDeneme.DAL.Models;
using KubisDeneme.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace KubisDeneme.Service
{
    public class YazarService : IYazarService
    {
        private readonly AppDbContext _context;
        public YazarService(AppDbContext context)
        {
            _context = context;
        }
        public List<YazarDTO> TumYazarlariGetir()
        {
            var yazarlar = _context.Yazarlar
                .Include(y => y.Ulke) // Ulke bilgisini 
                .Include(y => y.KitapYazarlar) // KitapYazarlar bilgisini 
                    .ThenInclude(ky => ky.Kitap) // Kitap bilgisini 
                .Select(yazar => new YazarDTO
                {
                    Id = yazar.Id,
                    Ad = yazar.Ad,
                    ISNI = yazar.ISNI,
                    DogumTarihi = yazar.DogumTarihi ?? default(DateTime),
                    EklenmeTarihi = yazar.EklenmeTarihi,
                    GuncellenmeTarihi = yazar.GuncellenmeTarihi,                  

                    //Foreign Key (Ülke)
                    UlkeId = yazar.UlkeId,
                    Ulke = yazar.Ulke != null ? new UlkeDTO
                    {
                        Id = yazar.Ulke.Id,
                        Ad = yazar.Ulke.Ad,
                    } : null,

                    //İlişkili yazarlar
                    KitapYazarlar = yazar.KitapYazarlar.Select(ky => new KitapYazarDTO
                    {
                        KitapId = ky.KitapId,
                        YazarId = ky.YazarId,
                        Kitap = ky.Kitap != null ? new KitapDTO
                        {
                            Id = ky.Kitap.Id,
                            KitapAdi = ky.Kitap.Ad,
                        } : null
                    }).ToList()
                })
                .ToList();

            return yazarlar;
        }
        public YazarDTO? YazarGetir(int id)
        {
            var yazar = _context.Yazarlar
                .Include(y => y.Ulke) // Ulke bilgisini dahil et
                .Include(y => y.KitapYazarlar) // KitapYazarlar bilgisini dahil et
                    .ThenInclude(ky => ky.Kitap) // Kitap bilgisini dahil et
                .FirstOrDefault(y => y.Id == id);
            if (yazar == null)
            {
                return null;
            }  
            return new YazarDTO
            {
                Id = yazar.Id,
                Ad = yazar.Ad,
                ISNI = yazar.ISNI,
                DogumTarihi = yazar.DogumTarihi,
                EklenmeTarihi = yazar.EklenmeTarihi,
                GuncellenmeTarihi = yazar.GuncellenmeTarihi,
                UlkeId = yazar.UlkeId,
                Ulke = yazar.Ulke != null ? new UlkeDTO
                {
                    Id = yazar.Ulke.Id,
                    Ad = yazar.Ulke.Ad,
                } : null,
                KitapYazarlar = yazar.KitapYazarlar.Select(ky => new KitapYazarDTO
                {
                    KitapId = ky.KitapId,
                    YazarId = ky.YazarId,
                    Kitap = ky.Kitap != null ? new KitapDTO
                    {
                        Id = ky.Kitap.Id,
                        KitapAdi = ky.Kitap.Ad,
                    } : null
                }).ToList()
            };
        }
        public void YazarEkle(YazarDTO yazarDTO)
        {
            if (yazarDTO.EklenmeTarihi > DateTime.UtcNow)
            {
                throw new ArgumentException("Gelecek tarihli kayıt oluşturulamaz");
            }

            if (_context.Yazarlar.Any(x => x.Ad.ToLower() == yazarDTO.Ad.ToLower()))
            {
                throw new ArgumentException("Bu yazar adı mevcut.", nameof(yazarDTO.Ad));
            }

            if (_context.Yazarlar.Any(x => x.ISNI == yazarDTO.ISNI))
            {
                throw new ArgumentException("Bu yazar ISNI mevcut.", nameof(yazarDTO.ISNI));
            }


            //Uyduruk oldu silinebilir
            if (yazarDTO.UlkeId == null || !_context.Ulkeler.Any(u => u.Id == yazarDTO.UlkeId))
            {
                throw new ArgumentException("Geçersiz ülke ID.", nameof(yazarDTO.UlkeId));
            }

            var guncellenmeTarihi = yazarDTO.GuncellenmeTarihi?.ToUniversalTime() ?? DateTime.UtcNow;
            var eklenmeTarihi = yazarDTO.EklenmeTarihi ?? DateTime.UtcNow;
            
            var yazar = new Yazar
            {
                Ad = yazarDTO.Ad,
                ISNI = yazarDTO.ISNI,
                DogumTarihi = DateTime.SpecifyKind(yazarDTO.DogumTarihi ?? DateTime.Now, DateTimeKind.Utc),
                EklenmeTarihi = eklenmeTarihi,
                GuncellenmeTarihi = guncellenmeTarihi,
                UlkeId = yazarDTO.UlkeId ?? 0
            };

            if (yazarDTO.EklenmeTarihi != null && yazarDTO.EklenmeTarihi != yazar.EklenmeTarihi)
            {
                yazar.EklenmeTarihi = yazarDTO.EklenmeTarihi.Value.ToUniversalTime();
            }

            _context.Yazarlar.Add(yazar);
            _context.SaveChanges();

            yazarDTO.Id = yazar.Id;
        }
        public void YazarGuncelle(YazarDTO yazarDTO)
        {
            if (yazarDTO.EklenmeTarihi > DateTime.UtcNow)
            {
                throw new ArgumentException("Gelecek tarihli kayıt oluşturulamaz");
            }

            var yazar = _context.Yazarlar.FirstOrDefault(x => x.Id == yazarDTO.Id);

            if (_context.Yazarlar.Any(x => x.Id != yazarDTO.Id && x.Ad.ToLower() == yazarDTO.Ad.ToLower()))
            {
                throw new ArgumentException("Bu yazar adı mevcut.", nameof(yazarDTO.Ad));
            }

            if (_context.Yazarlar.Any(x => x.Id != yazarDTO.Id && x.ISNI == yazarDTO.ISNI))
            {
                throw new ArgumentException("Bu yazar ISNI mevcut.", nameof(yazarDTO.ISNI));
            }

            // Uyduruk oldu silinebilir
            if (yazarDTO.UlkeId == null || !_context.Ulkeler.Any(u => u.Id == yazarDTO.UlkeId))
            {
                throw new ArgumentException("Geçersiz ülke ID.", nameof(yazarDTO.UlkeId));
            }

            yazar.Ad = yazarDTO.Ad;
            yazar.ISNI = yazarDTO.ISNI;
            yazar.DogumTarihi = yazarDTO.DogumTarihi.HasValue ? DateTime.SpecifyKind(yazarDTO.DogumTarihi.Value, DateTimeKind.Utc) : (DateTime?)null;
            yazar.GuncellenmeTarihi = yazarDTO.GuncellenmeTarihi?.ToUniversalTime() ?? DateTime.UtcNow;

            if (yazarDTO.EklenmeTarihi != null && yazarDTO.EklenmeTarihi != yazar.EklenmeTarihi)
            {
                yazar.EklenmeTarihi = yazarDTO.EklenmeTarihi.Value.ToUniversalTime();
            }

            yazar.UlkeId = yazarDTO.UlkeId ?? 0;
            _context.SaveChanges();               
        }
        public void YazarSil(int id)
        {
            var yazar = _context.Yazarlar.Find(id);
            if (yazar == null)
                return;
            _context.Yazarlar.Remove(yazar);
            _context.SaveChanges();
        }      
    }
}
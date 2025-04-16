using FluentValidation;
using KubisDeneme.DTO;

namespace KubisDeneme.Service.Validators
{
    public class KitapValidator : AbstractValidator<KitapDTO>
    {
        private readonly IKitapService _kitapService;
        public KitapValidator(IKitapService kitapService)
        {
            RuleFor(x => x.KitapAdi)
                .NotEmpty().
                WithMessage("Kitap adı boş bırakılamaz!");

            RuleFor(x => x.KitapAdi)
                .MaximumLength(100).
                WithMessage("Kitap adı en fazla 100 karakter olabilir!");

            RuleFor(x => x.İlkYayinYili)
                .Must(x => x.ToString().Length == 4 && int.TryParse(x.ToString(), out _))
                .WithMessage("İlk yayın yılı 4 haneli bir sayı olmalıdır!");

            RuleFor(x => x.ISBN)
                .NotEmpty()
                .WithMessage("Kitap ISBN boş bırakılamaz!");

            RuleFor(x => x.EklenmeTarihi)
               .LessThanOrEqualTo(DateTime.UtcNow)
               .WithMessage("Eklenme tarihi bugünden sonraki bir tarih olamaz!");

            // Yeni kural: KitapYazarlar listesinin boş olmamasını kontrol et
            RuleFor(x => x.KitapYazarlar)
                .NotEmpty()
                .WithMessage("Kitap için en az bir yazar seçilmelidir!");

            // Yeni kural: KitapKitapTurleri listesinin boş olmamasını kontrol et
            RuleFor(x => x.KitapKitapTurleri)
                .NotEmpty()
                .WithMessage("Kitap için en az bir kitap türü seçilmelidir!");
        }
    }
}

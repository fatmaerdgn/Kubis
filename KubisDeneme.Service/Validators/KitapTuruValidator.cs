using FluentValidation;
using KubisDeneme.DTO;

namespace KubisDeneme.Service.Validators
{
    public class KitapTuruValidator : AbstractValidator<KitapTuruDTO>
    {
        public KitapTuruValidator() {
            RuleFor(x => x.Ad)
                .NotEmpty()
                .WithMessage("Kitap türü adı boş bırakılamaz!");

            RuleFor(x => x.Ad)
                .MaximumLength(20)
                .WithMessage("Kitap türü adı en fazla 20 karakter olabilir!");

            RuleFor(x => x.EklenmeTarihi)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Eklenme tarihi bugünden sonraki bir tarih olamaz!");
        }
    }
}

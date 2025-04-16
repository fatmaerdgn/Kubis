using FluentValidation;
using KubisDeneme.DTO;

namespace KubisDeneme.Service.Validators
{
    public class YazarValidator : AbstractValidator<YazarDTO>
    {
        public YazarValidator(IYazarService yazarService)
        {            
            RuleFor(x => x.Ad)
                .NotEmpty().
                WithMessage("Yazar adı boş bırakılamaz!");

            RuleFor(x => x.Ad)
                .MaximumLength(50).
                WithMessage("Yazar adı en fazla 50 karakter olabilir!");

            RuleFor(x => x.DogumTarihi)
                .NotNull()
                .WithMessage("Doğum tarihi boş bırakılamaz!");

            RuleFor(x => x.DogumTarihi)
                .LessThanOrEqualTo(DateTime.Now.Date)
                .WithMessage("Doğum tarihi bugünden ileri bir tarih olamaz!");

            RuleFor(x => x.EklenmeTarihi)
               .LessThanOrEqualTo(DateTime.UtcNow)
               .WithMessage("Eklenme tarihi bugünden sonraki bir tarih olamaz!");

            RuleFor(x => x.ISNI)
                .NotEmpty()
                .WithMessage("Yazar ISNI boş bırakılamaz!");

            RuleFor(x => x.UlkeId)
                .NotNull()
                .WithMessage("Ülke seçimi boş bırakılamaz!");
        }
    }
}

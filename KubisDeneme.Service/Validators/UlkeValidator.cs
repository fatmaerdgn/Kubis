using FluentValidation;
using KubisDeneme.DTO;

namespace KubisDeneme.Service.Validators
{
    public class UlkeValidator : AbstractValidator<UlkeDTO>
    {
        public UlkeValidator() 
        {
            RuleFor(x => x.Ad)
                .NotEmpty()
                .WithMessage("Ad alanı boş olamaz!");

            RuleFor(x => x.Ad)
                .MaximumLength(40)
                .WithMessage("Ad alanı en fazla 40 karakter olabilir!");

            RuleFor(x => x.EklenmeTarihi)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Eklenme tarihi bugünden sonraki bir tarih olamaz!");
        }
    }
}

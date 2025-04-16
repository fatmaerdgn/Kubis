using KubisDeneme.DTO;
using Microsoft.AspNetCore.Mvc;
using KubisDeneme.Service;
using FluentValidation;
using FluentValidation.Results;

namespace KubisDeneme.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitapController : ControllerBase
    {
        private readonly IKitapService _kitapService;
        private readonly IValidator<KitapDTO> _validator;
        public KitapController(IKitapService kitapService, IValidator<KitapDTO> validator)
        {
            _kitapService = kitapService;
            _validator = validator;
        }

        [HttpGet]
        public ActionResult<List<KitapDTO>> GetKitaplar()
        {
            return Ok(_kitapService.TumKitaplariGetir());
        }

        [HttpGet("{id}")]
        public ActionResult GetKitap(int id)
        {
            var kitap = _kitapService.KitapGetir(id);
            if (kitap == null)
            {
                return NotFound("Kitap bulunamadı.");
            }
            return Ok(kitap);
        }

        [HttpPost("ekle")]
        public ActionResult<KitapDTO> Ekle([FromBody] KitapDTO kitapDTO)
        {
            ValidationResult validationResult = _validator.Validate(kitapDTO);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { errors = errorMessages });
            }

            // Tarih UTC kontrolü (deneme amaçlı silinebilir)
            if (kitapDTO.EklenmeTarihi?.Kind == DateTimeKind.Local)
            {
                kitapDTO.EklenmeTarihi = kitapDTO.EklenmeTarihi.Value.ToUniversalTime();
            }

            try
            {
                _kitapService.KitapEkle(kitapDTO);
                return CreatedAtAction(nameof(GetKitap), new { id = kitapDTO.Id }, kitapDTO);
            }
            catch (ArgumentException ex)
            {
                // Tek bir hata mesajını bile List<string> olarak döndür
                return BadRequest(new List<string> { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpPut]
        public ActionResult<KitapDTO> PutKitap([FromBody] KitapDTO kitapDTO)
        {
            ValidationResult validationResult = _validator.Validate(kitapDTO);

            if (!validationResult.IsValid)
            {
                // Sadece hata mesajlarını al
                var errorMessages = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { errors = errorMessages }); // Hata mesajlarını dizi olarak döndür
            }

            // Tarih UTC kontrolü (deneme amaçlı silinebilir)
            if (kitapDTO.EklenmeTarihi?.Kind == DateTimeKind.Local)
            {
                kitapDTO.EklenmeTarihi = kitapDTO.EklenmeTarihi.Value.ToUniversalTime();
            }

            try
            {
                _kitapService.KitapGuncelle(kitapDTO);
                return Ok(kitapDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new List<string> { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpDelete]
        public ActionResult DeleteKitap(int id)
        {
            _kitapService.KitapSil(id);
            return Ok();
        }
    }
}

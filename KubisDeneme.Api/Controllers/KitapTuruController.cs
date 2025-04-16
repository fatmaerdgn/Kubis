using KubisDeneme.DTO;
using Microsoft.AspNetCore.Mvc;
using KubisDeneme.Service;
using FluentValidation;
using FluentValidation.Results;

namespace KubisDeneme.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitapTuruController : ControllerBase
    {
        private readonly IKitapTuruService _kitapTuruService;
        private readonly IValidator<KitapTuruDTO> _validator;
        public KitapTuruController(IKitapTuruService kitapTuruService, IValidator<KitapTuruDTO> validator)
        {
            _kitapTuruService = kitapTuruService;
            _validator = validator;
        }

        [HttpGet]
        public ActionResult GetKitapTurleri()
        {
            return Ok(_kitapTuruService.TumKitapTurleriniGetir());
        }

        [HttpGet("{id}")]
        public ActionResult<KitapTuruDTO> GetKitapTuru(int id)
        {
            var kitapTuru = _kitapTuruService.KitapTuruGetir(id);
            if (kitapTuru == null)
            {
                return NotFound("Kitap türü bulunamadı");
            }
            return Ok(kitapTuru);
        }

        [HttpPost("ekle")]
        public ActionResult<KitapTuruDTO> Ekle([FromBody] KitapTuruDTO kitapTuruDTO)
        {
            ValidationResult validationResult = _validator.Validate(kitapTuruDTO);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new {errors = errorMessages});
            }

            // Tarih UTC kontrolü (deneme amaçlı silinebilir)
            if (kitapTuruDTO.EklenmeTarihi?.Kind == DateTimeKind.Local)
            {
                kitapTuruDTO.EklenmeTarihi = kitapTuruDTO.EklenmeTarihi.Value.ToUniversalTime();
            }

            try
            {
                _kitapTuruService.KitapTuruEkle(kitapTuruDTO);
                return CreatedAtAction(nameof(GetKitapTuru), new { id = kitapTuruDTO.Id }, kitapTuruDTO);
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
        public ActionResult PutKitapTuru(KitapTuruDTO kitapTuruDTO)
        {
            ValidationResult validationResult = _validator.Validate(kitapTuruDTO);

            if (!validationResult.IsValid)
            {
                // Sadece hata mesajlarını al
                var errorMessages = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { errors = errorMessages }); // Hata mesajlarını dizi olarak döndür
            }

            // Tarih UTC kontrolü (deneme amaçlı silinebilir)
            if (kitapTuruDTO.EklenmeTarihi?.Kind == DateTimeKind.Local)
            {
                kitapTuruDTO.EklenmeTarihi = kitapTuruDTO.EklenmeTarihi.Value.ToUniversalTime();
            }

            try
            {
                _kitapTuruService.KitapTuruGuncelle(kitapTuruDTO);
                return Ok(kitapTuruDTO);
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
        public ActionResult DeleteKitapTuru(int id)
        {
            _kitapTuruService.KitapTuruSil(id);
            return Ok();
        }
    }
}

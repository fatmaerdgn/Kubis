using KubisDeneme.DTO;
using Microsoft.AspNetCore.Mvc;
using KubisDeneme.Service;
using FluentValidation;
using FluentValidation.Results;

namespace KubisDeneme.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YazarController : ControllerBase
    {
        private readonly IYazarService _yazarService;
        private readonly IValidator<YazarDTO> _validator;
        public YazarController(IYazarService yazarService, IValidator<YazarDTO> validator)
        {
            _yazarService = yazarService;
            _validator = validator;
        }

        [HttpGet]
        public ActionResult GetYazarlar()
        {
            return Ok(_yazarService.TumYazarlariGetir());
        }

        [HttpGet("{id}")]
        public ActionResult GetYazar(int id)
        {
            var yazar = _yazarService.YazarGetir(id);
            if (yazar == null)
            {
                return NotFound("Yazar bulunamadı.");
            }
            return Ok(yazar);
        }

        [HttpPost("ekle")]
        public ActionResult<YazarDTO> Ekle([FromBody] YazarDTO yazarDTO)
        {
            ValidationResult validationResult = _validator.Validate(yazarDTO);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { errors = errorMessages });
            }

            // Tarih UTC kontrolü (deneme amaçlı silinebilir)
            if (yazarDTO.EklenmeTarihi?.Kind == DateTimeKind.Local)
            {
                yazarDTO.EklenmeTarihi = yazarDTO.EklenmeTarihi.Value.ToUniversalTime();
            }

            try
            {
                _yazarService.YazarEkle(yazarDTO);
                return CreatedAtAction(nameof(GetYazar), new { id = yazarDTO.Id }, yazarDTO);
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
        public ActionResult PutYazar(YazarDTO yazarDTO)
        {
            ValidationResult validationResult = _validator.Validate(yazarDTO);

            if (!validationResult.IsValid)
            {
                // Sadece hata mesajlarını al
                var errorMessages = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { errors = errorMessages }); // Hata mesajlarını dizi olarak döndür
            }

            // Tarih UTC kontrolü (deneme amaçlı silinebilir)
            if (yazarDTO.EklenmeTarihi?.Kind == DateTimeKind.Local)
            {
                yazarDTO.EklenmeTarihi = yazarDTO.EklenmeTarihi.Value.ToUniversalTime();
            }

            try
            {
                _yazarService.YazarGuncelle(yazarDTO);
                return Ok(yazarDTO);
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
        public ActionResult DeleteYazar(int id)
        {
            _yazarService.YazarSil(id);
            return Ok();
        }
    }
}

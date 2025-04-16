using KubisDeneme.DTO;
using Microsoft.AspNetCore.Mvc;
using KubisDeneme.Service;
using FluentValidation;
using FluentValidation.Results;

namespace KubisDeneme.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UlkeController : ControllerBase
    {
        private readonly IUlkeService _ulkeService;
        private readonly IValidator<UlkeDTO> _validator;

        public UlkeController(IUlkeService ulkeService, IValidator<UlkeDTO> validator)
        {
            _ulkeService = ulkeService;
            _validator = validator;
        }

        [HttpGet]
        public ActionResult<List<UlkeDTO>> GetUlkeler()
        {
            return Ok(_ulkeService.TumUlkeleriGetir());
        }

        [HttpGet("{id}")]
        public ActionResult<UlkeDTO> GetUlke(int id)
        {
            var ulke = _ulkeService.UlkeGetir(id);
            if (ulke == null)
            {
                return NotFound("Ülke bulunamadı.");
            }
            return Ok(ulke);
        }

        [HttpPost("ekle")]
        public ActionResult<UlkeDTO> Ekle([FromBody] UlkeDTO ulkeDTO)
        {
            ValidationResult validationResult = _validator.Validate(ulkeDTO);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(errorMessages);
            }

            // Tarih UTC kontrolü (deneme amaçlı silinebilir)
            if (ulkeDTO.EklenmeTarihi?.Kind == DateTimeKind.Local)
            {
                ulkeDTO.EklenmeTarihi = ulkeDTO.EklenmeTarihi.Value.ToUniversalTime();
            }

            try
            {
                _ulkeService.UlkeEkle(ulkeDTO);
                return CreatedAtAction(nameof(GetUlke), new { id = ulkeDTO.Id }, ulkeDTO);
            }
            catch (ArgumentException ex)
            {
                // Tek bir hata mesajını bile List<string> olarak döndür
                return BadRequest(new List<string> { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new List<string> { $"Sunucu hatası: {ex.Message}" });
            }
        }

        [HttpPut]
        public ActionResult PutUlke(UlkeDTO ulkeDTO)
        {
            ValidationResult validationResult = _validator.Validate(ulkeDTO);

            if (!validationResult.IsValid)
            {
                // Sadece hata mesajlarını al
                var errorMessages = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(errorMessages); // Hata mesajlarını dizi olarak döndür
            }

            // Tarih UTC kontrolü (deneme amaçlı silinebilir)
            if (ulkeDTO.EklenmeTarihi?.Kind == DateTimeKind.Local)
            {
                ulkeDTO.EklenmeTarihi = ulkeDTO.EklenmeTarihi.Value.ToUniversalTime();
            }

            try
            {
                _ulkeService.UlkeGuncelle(ulkeDTO);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                // Tek bir hata mesajını bile List<string> olarak döndür
                return BadRequest(new List<string> { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new List<string> { $"Sunucu hatası: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")] 
        public ActionResult DeleteUlke(int id)
        {
            _ulkeService.UlkeSil(id);
            return Ok();
        }
    }
}
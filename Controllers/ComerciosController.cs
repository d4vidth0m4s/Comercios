using System.Globalization;
using Comercios.Application.DTOs.Comercios.Response;
using Comercios.Application.DTOs.CrearComercio.Resquest;
using Comercios.Application.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comercios.Controllers
{
    [ApiController]
    [Route("Comercios")]
    public class ComerciosController : ControllerBase
    {
        private readonly IComercioServices _comercioServices;

        public ComerciosController(IComercioServices comercioServices)
        {
            _comercioServices = comercioServices;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ComerciosPaginadosResponseDto>> ListarComercios(
            [FromQuery] int pagina = 1,
            [FromQuery] int limite = 20,
            [FromQuery(Name = "Calificacion")] string? calificacion = null)
        {
            if (pagina < 1)
                return BadRequest(new { message = "El parametro 'pagina' debe ser mayor o igual a 1." });

            if (limite < 1 || limite > 100)
                return BadRequest(new { message = "El parametro 'limite' debe estar entre 1 y 100." });

            decimal? calificacionMinima = null;
            if (!string.IsNullOrWhiteSpace(calificacion))
            {
                if (!TryParseCalificacionMinima(calificacion, out var parsed))
                    return BadRequest(new { message = "El parametro 'Calificacion' es invalido. Ejemplo valido: '>=4' o '=>4'." });

                calificacionMinima = parsed;
            }

            var response = await _comercioServices.ObtenerComerciosAsync(pagina, limite, calificacionMinima);
            return Ok(response);
        }

        [HttpPost("CrearComercios")]
        [Authorize]
        public async Task<ActionResult<string>> Create([FromBody] CrearComercioResquestDto dto)
        {
            try
            {
                var userIdClaim = Request.GetUserId();
                var id = await _comercioServices.CreateAsync(dto, userIdClaim);
                return StatusCode(201, id);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut("ActualizarComercio")]
        [Authorize]
        public async Task<ActionResult> Update([FromBody] CrearComercioResquestDto dto)
        {
            try
            {
                var userIdClaim = Request.GetUserId();
                var updated = await _comercioServices.UpdateAsync(dto, userIdClaim);
                return updated ? NoContent() : NotFound();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("ok")]
        public IActionResult Health()
        {
            var userIdClaim = Request.GetUserId();
            return Ok(new
            {
                userid = userIdClaim,
                service = "Comercios",
                status = "OK"
            });
        }

        private static bool TryParseCalificacionMinima(string rawValue, out decimal value)
        {
            value = 0;
            var normalized = rawValue.Trim().Replace(" ", string.Empty);

            if (normalized.StartsWith(">=") || normalized.StartsWith("=>"))
                normalized = normalized[2..];
            else if (normalized.StartsWith(">") || normalized.StartsWith("="))
                normalized = normalized[1..];

            return decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out value)
                || decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.CurrentCulture, out value);
        }
    }

    public static class HttpRequestExtensions
    {
        public static string GetUserId(this HttpRequest request)
        {
            return request.Headers["X-User-Id"].ToString();
        }
    }
}

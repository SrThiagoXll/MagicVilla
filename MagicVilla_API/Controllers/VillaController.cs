using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio;
using MagicVilla_API.Repositorio.IRpositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepositorio _villaRe;
        private readonly IMapper _mapper;
        protected ApiResponce _responce; 

        public VillaController(ILogger<VillaController> logger,IVillaRepositorio villaRe, IMapper mapper)
        {
            _logger = logger;
            _villaRe = villaRe;
            _mapper = mapper;
            _responce = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ApiResponce>> GetVillas()
        {
            try
            {
                _logger.LogInformation("otener datos");

                IEnumerable<Villa> villaList = await _villaRe.ObtenerTodos();

                _responce.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
                _responce.statusCode = HttpStatusCode.OK;

                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.IsExitoso= false;
                _responce.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _responce;
        }

        [HttpGet("{id}",Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponce>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Villa con Id " + id);
                    _responce.IsExitoso = false;
                    _responce.statusCode =HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var villa = await _villaRe.Obtener(x => x.Id == id);

                if (villa == null)
                {
                    _responce.IsExitoso = false;
                    _responce.statusCode =HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Resultado = _mapper.Map<VillaDto>(villa);
                _responce.statusCode = HttpStatusCode.OK;

                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.IsExitoso = false;
                _responce.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _responce;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponce>> Create([FromBody] VillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _villaRe.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                Villa modelo = _mapper.Map<Villa>(createDto);
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;

                await _villaRe.Crear(modelo);
                _responce.Resultado = modelo;
                _responce.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = modelo.Id },_responce);
            }
            catch (Exception ex)
            {
                _responce.IsExitoso = false;
                _responce.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _responce;            
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _responce.IsExitoso = false;
                    _responce.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                var villa = await _villaRe.Obtener(v => v.Id == id);
                if (villa == null)
                {
                    _responce.IsExitoso = false;
                    _responce.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }
                await _villaRe.Remove(villa);

                _responce.statusCode = HttpStatusCode.NoContent;

                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.IsExitoso = false;
                _responce.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_responce);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    _responce.IsExitoso = false;
                    _responce.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                Villa modelo = _mapper.Map<Villa>(updateDto);

                await _villaRe.Actualizar(modelo);

                _responce.statusCode = HttpStatusCode.NoContent;
                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.IsExitoso = false;
                _responce.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return Ok(_responce);

        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    _responce.IsExitoso = false;
                    _responce.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                var villa = await _villaRe.Obtener(v => v.Id == id, tracked: false);

                VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

                if (villa == null)
                {
                    return BadRequest();
                }

                patchDto.ApplyTo(villaDto, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Villa modelo = _mapper.Map<Villa>(villaDto);

                await _villaRe.Actualizar(modelo);
                _responce.statusCode = HttpStatusCode.NoContent;

                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.IsExitoso = false;
                _responce.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return Ok(_responce);
            
        }
    }
}

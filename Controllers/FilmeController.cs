using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dto;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;

namespace FilmesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public FilmeController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
                  
        [HttpGet]
        public IEnumerable<ReadFilmeDto> ConsultaFilme([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
        }

        [HttpGet("{id}")]
        public IActionResult ConsultaFilmePorId(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

            if (filme == null)
            {
                return NotFound();
            }

            var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
            return Ok(filmeDto);
        }


        ///<summary>
        ///Adiciona um filme ao banco de dados
        ///</summary>
        ///<param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
        ///<returns>IActionResult</returns>
        ///<response code="201">Caso a inserção seja realizada com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
        {
            Filme filme = _mapper.Map<Filme>(filmeDto);            
            _context.Filmes.Add(filme);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ConsultaFilmePorId), new { id = filme.Id }, filme);
        }



        ///<summary>
        ///Atualiza o objeto completo no banco de dados
        ///</summary>
        ///<param name="id">Id do filme</param>
        ///<param name="filmeDto">Objeto com os novos dados</param>
        ///<returns>IActionResult</returns>
        ///<response code="204">Caso a atualização seja realizada com sucesso</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

            if (filme == null)
            {
                return NotFound();
            }

            _mapper.Map(filmeDto, filme);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

            if (filme == null)
            {
                return NotFound();
            }

            //Validando se as propriedades são informações válidas do modelo
            var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
            patch.ApplyTo(filmeParaAtualizar, ModelState);

            if (!TryValidateModel(filmeParaAtualizar))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(filmeParaAtualizar, filme);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletaFilme(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

            if (filme == null)
            {
                return NotFound();
            }

            _context.Remove(filme);
            _context.SaveChanges(); 
            return NoContent();
        }
    }
}

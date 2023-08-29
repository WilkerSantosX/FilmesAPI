using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dto;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;

namespace FilmesAPI.Controllers
{
    ///<summary>
    ///API que disponibiliza métodos RestFufll para Filmes
    ///</summary>
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        ///<summary>
        ///API que disponibiliza métodos RestFufll para Filmes
        ///</summary>
        public FilmeController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        ///<summary>
        ///Consulta filmes no banco de dados
        ///</summary>
        ///<param name="skip">skip</param>
        ///<param name="take">take</param>
        ///<returns>IActionResult</returns>
        ///<response code="200">Retorna uma listta de filme</response>                  
        [HttpGet("Consultar")]
        public IEnumerable<ReadFilmeDto> Consultar([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
        }

        ///<summary>
        ///Consulta um filme através do seu Id no banco de dados
        ///</summary>
        ///<param name="id">Id do filme</param>
        ///<returns>IActionResult</returns>
        ///<response code="200">Retorna o objeto filme</response>
        [HttpGet("ConsultarPorId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ConsultarPorId(int id)
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
        [HttpPost("Adicionar")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Adicionar([FromBody] CreateFilmeDto filmeDto)
        {
            Filme filme = _mapper.Map<Filme>(filmeDto);            
            _context.Filmes.Add(filme);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ConsultarPorId), new { id = filme.Id }, filme);
        }



        ///<summary>
        ///Atualiza um filme no banco de dados
        ///</summary>
        ///<param name="id">Id do filme</param>
        ///<param name="filmeDto">Objeto com os novos dados</param>
        ///<returns>IActionResult</returns>
        ///<response code="204">Caso a atualização seja realizada com sucesso</response>
        [HttpPut("Atualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Atualizar(int id, [FromBody] UpdateFilmeDto filmeDto)
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


        ///<summary>
        ///Atualiza um filme no banco de dados
        ///</summary>
        ///<param name="id">Id do filme</param>
        ///<param name="patch">Propriedades com os novos dados</param>
        ///<returns>IActionResult</returns>
        ///<response code="204">Caso a atualização seja realizada com sucesso</response>
        [HttpPatch("Atualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Atualizar(int id, JsonPatchDocument<UpdateFilmeDto> patch)
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


        ///<summary>
        ///Deleta um filme do banco de dados
        ///</summary>
        ///<param name="id">Id para </param>
        ///<returns>IActionResult</returns>
        ///<response code="204">Caso a remoção seja realizada com sucesso</response>
        [HttpDelete("Deletar/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Deletar(int id)
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

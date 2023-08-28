using FilmesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;

namespace FilmesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private static List<Filme> filmes = new List<Filme>();
        private static int id = 1;

        [HttpGet]
        public IEnumerable<Filme> ConsultaFilme([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return filmes.Skip(skip).Take(take);
        }

        [HttpGet("{id}")]
        public IActionResult ConsultaFilmePorId(int id)
        {
            var filme = filmes.FirstOrDefault(filme => filme.Id == id);

            if (filme == null)
            {
                return NotFound();
            }

            return Ok(filme);
        }

        [HttpPost]
        public IActionResult AdicionaFilme([FromBody] Filme filme)
        {
            filme.Id = id++;
            filmes.Add(filme);

            return CreatedAtAction(nameof(ConsultaFilmePorId), new { id = filme.Id }, filme);
        }


    }
}

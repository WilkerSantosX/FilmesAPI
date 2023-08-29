using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dto
{
    public class ReadFilmeDto
    {
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public int Duracao { get; set; }
        public DateTime DataConsulta { get; set;} = DateTime.Now;
    }
}

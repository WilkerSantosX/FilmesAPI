using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dto
{
    public class CreateFilmeDto
    {
        [Required(ErrorMessage = "O Titulo do filme é obrigatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O Genero do filme é obrigatorio")]
        [StringLength(50, ErrorMessage = "O tamanho do genêro não pode exceder 50 caracteres")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "A Duracao do filme é obrigatorio")]
        [Range(1, 360, ErrorMessage = "A duração do filme deve ter entre 1 minuto e 360")]
        public int Duracao { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models
{
    public class Filme
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Titulo do filme é obrigatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O Genero do filme é obrigatorio")]
        [MaxLength(50, ErrorMessage = "O tamanho do genêro não pode exceder 50 caracteres")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "A Duracao do filme é obrigatorio")]
        [Range(1, 360, ErrorMessage = "A duração do filme deve ter entre 1 minuto e 360")]
        public int Duracao { get; set; }
    }
}

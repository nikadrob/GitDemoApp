using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.API.Data
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obvezan unos naziva!")]
        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Poster { get; set; }

        [ForeignKey(nameof(Director))]
        public int DirectorId { get; set; }

        public Director Director { get; set; }

        [ForeignKey(nameof(Genre))]
        public int GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}
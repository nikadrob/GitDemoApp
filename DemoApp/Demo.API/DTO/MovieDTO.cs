using Demo.API.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.DTO
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
        public int DirectorId { get; set; }
        public DirectorDTO Director { get; set; }
        public int GenreId { get; set; }
        public virtual GenreDTO Genre { get; set; }
    }

    public class MovieCreateDTO
    {
        [Required]
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string Poster { get; set; }
        [Required]
        public int DirectorId { get; set; }
        [Required]
        public int GenreId { get; set; }

    }

    public class MovieUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string Poster { get; set; }
        [Required]
        public int DirectorId { get; set; }
        [Required]
        public int GenreId { get; set; }
    }
}

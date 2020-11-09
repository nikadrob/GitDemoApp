using Demo.API.Data;
using System;
using System.Collections.Generic;
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

        public Director Director { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}

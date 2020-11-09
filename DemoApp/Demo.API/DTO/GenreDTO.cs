using Demo.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.DTO
{
    public class GenreDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IList<Movie> Movies { get; set; }
    }
}

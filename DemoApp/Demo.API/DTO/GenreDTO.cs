using Demo.API.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.DTO
{
    public class GenreDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IList<MovieDTO> Movies { get; set; }
    }

    public class GenreCreateDTO
    {
        [Required]
        public string Name { get; set; }

    }

    public class GenreUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

    }
}

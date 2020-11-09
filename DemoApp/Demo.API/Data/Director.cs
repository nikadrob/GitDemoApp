using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.API.Data
{
    public class Director
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obvezan unos imena!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Obvezan unos prezimena!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Obvezan unos datuma rođenja!")]
        public DateTime DateOfBirth { get; set; }

        public virtual IList<Movie> Movies { get; set; }
    }
}
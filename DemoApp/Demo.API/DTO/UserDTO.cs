using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.DTO
{
    public class UserDTO
    {
        [Required]
        [EmailAddress]
        public string EmailAdress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "You pass is limited 6 - 15 chars.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}

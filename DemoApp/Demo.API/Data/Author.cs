using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Data
{
    [Table("Authors")]
    public partial class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
            
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Bio { get; set; }

        public virtual IList<Book> Books { get; set; }
    }
}

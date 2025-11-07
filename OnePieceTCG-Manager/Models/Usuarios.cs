using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnePieceTCG_Manager.Models
{
    [Table("Usuarios")]
    public class Usuarios
    {
        [Key]
        public string codUsu { get; set; }
        public string userName { get; set; }
        public string passwd { get; set; }
        public string hostname { get; set; }
    }
}

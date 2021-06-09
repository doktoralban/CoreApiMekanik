using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiMekanik.Models
{
    public class User
    {
        [Key]
        public int USERID { get; set; }
        [Key]
        public string USERNAME { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string PASSWORD { get; set; }
        //[Required]
        public bool ISACTIVE { get; set; }
        public string PHOTOPATH { get; set; }
        public string ROLEGROUP { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EMAIL{ get; set; }
        public string FIRSTNAME { get;  set; }
        public string  LASTNAME { get; set; }
    }
}

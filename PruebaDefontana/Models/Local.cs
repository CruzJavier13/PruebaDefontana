using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaDefontana.Models
{
    internal class Local
    {
        [Key]
        public int ID_Local { set; get; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
    }
}

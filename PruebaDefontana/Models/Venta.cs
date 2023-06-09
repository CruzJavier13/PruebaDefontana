using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaDefontana.Models
{
    internal class Venta
    {
        [Key]
        public long ID_Venta { get; set; }
        public int Total { get; set; }
        public DateTime Fecha { get; set; }

        [ForeignKey(nameof(ID_Venta))]
        public long ID_Local { get; set; }
    }
}

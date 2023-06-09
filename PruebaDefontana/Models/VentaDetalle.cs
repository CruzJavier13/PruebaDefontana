using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaDefontana.Models
{
    internal class VentaDetalle
    {
        [Key]
        public int ID_Detalle { get; set; }

        [ForeignKey(nameof(ID_Detalle))]
        public int ID_Venta { get; set; }
        public decimal Precio_Unitario { get; set; }
        public int Cantidad { get; set; }
        public int TotalLinea { get; set; }

        [ForeignKey(nameof(ID_Detalle))]
        public int ID_Producto { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaDefontana.Models
{
    internal class Producto
    {
        [Key]
        public long ID_Producto { get; set; }
        public string Nombre { get; set; }
        public string codigo { get; set; }

        [ForeignKey(nameof(ID_Producto))]
        public int ID_Marca { get; set; }
        public string Modelo { get; set; }
        public decimal Costo { get; set;}
    }
}

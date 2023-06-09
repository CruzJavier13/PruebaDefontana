using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaDefontana.Models
{
    internal class PruebaDB : DbContext
    {
        public DbSet<Producto> Producto { get; set; }
        public DbSet<VentaDetalle> VentaDetalle { get; set; }
        public DbSet<Venta> Venta { get; set; }
        public DbSet<Marca> Marca { get; set; }
        public DbSet<Local> Local { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string connectionString = Configuration.GetConnectionString("appContext");
            //optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseSqlServer(@"Server=lab-defontana.caporvnn6sbh.us-east-1.rds.amazonaws.com,1433;Database=Prueba;User ID=ReadOnly;Password=d*3PSf2MmRX9vJtA5sgwSphCVQ26*T53uU;TrustServerCertificate=True");
        }
    }
}

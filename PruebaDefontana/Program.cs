using PruebaDefontana.Models;

namespace PruebaDefontana
{
    class MostrarResultado
    {
        private readonly PruebaDB db = new PruebaDB();
        public static void Main(String[] args)
        {
            MostrarResultado mostrarResultado = new MostrarResultado();
            mostrarResultado.MostrarVentas();
        }
        public void MostrarVentas()
        {
            DateTime fechaLimite = DateTime.Now.AddDays(-30);

            List<Venta> ventas = db.Venta.ToList();
            Console.WriteLine("Total de registro en la tabla Venta: "+ventas.Count);
            Console.WriteLine("------------------------------------------------------------");
            //Ventas de los ultimos 30 días
            var datos = ventas.Where(v => v.Fecha >= fechaLimite).ToList();
            foreach (var d in datos.ToList())
            {
                Console.WriteLine(" ID: "+d.ID_Venta + " Fecha: " + d.Fecha + " ID Local:" + d.ID_Local + " Total: " + d.Total);
            }
            Console.WriteLine("------------------------------------------------------------");
            //Total ventas de los últimos 30 días
            var total = datos.Sum(v => v.Total);
            Console.WriteLine("Total vantas de los últimos 30 dias: "+total);
            Console.WriteLine("------------------------------------------------------------");
            //Venta mús alta
            var masAlta = datos.OrderByDescending(v => v.Total).Take(1)
                .Select(v => new Venta
                {
                    ID_Venta = v.ID_Venta,
                    Fecha = v.Fecha,
                    Total = v.Total,
                }).ToList();
            Console.WriteLine("Venta mas alta!!!!");
            foreach( var d in masAlta.ToList()) {
                Console.WriteLine("ID: " + d.ID_Venta + " Fecha: "+ d.Fecha + " Total: "+ d.Total) ;
             }
            Console.WriteLine("------------------------------------------------------------");
            Console.ReadKey();
        }

    }
}



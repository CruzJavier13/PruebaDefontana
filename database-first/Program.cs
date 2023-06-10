using database_first.Models;
using System.Security.Cryptography;
using System.Threading.Tasks.Dataflow;

PruebaContext context = new PruebaContext();

List<Ventum> ventas = context.Venta.ToList();

DateTime fechaLimite = DateTime.Now.AddDays(-30);
var datos = ventas.Where(v => v.Fecha >= fechaLimite).ToList();
var total = datos.Sum(v => v.Total);
var cantidadVenta = datos.Count;
Console.WriteLine("-----------------------------------------------------------------------------------");
Console.WriteLine("Total de las ventas: "+total+ " Cantidad total de las ventas: "+cantidadVenta);
Console.WriteLine("-----------------------------------------------------------------------------------");
var ventaMontaMasAlto = datos.OrderByDescending(v => v.Total).Take(1)
                .Select(v => new Ventum
                {
                    Fecha = v.Fecha,
                    Total = v.Total
                }).ToList();
var dia = ventaMontaMasAlto[0].Fecha.ToString("dddd"); var hora = ventaMontaMasAlto[0].Fecha.Hour; var masAlta = ventaMontaMasAlto[0].Total;
Console.WriteLine("Laventa mas alta se realizo el día: {0} Hora: {1}, el total fue de: {2}", dia, hora, masAlta.ToString());
Console.WriteLine("-----------------------------------------------------------------------------------");
var producto = from t1 in context.Productos join t2 in context.VentaDetalles on t1.IdProducto equals t2.IdProducto join t3 in context.Venta on  t2.IdVenta equals t3.IdVenta
               group t3.Total by t1.IdProducto into g
               orderby g.Sum() descending
               select new
               {
                   IdProducto = g.Key,
                   TotalSum = g.Sum()
               };
var productoMasVendido = producto.FirstOrDefault();
Console.WriteLine($"El producto más vendido es el : {productoMasVendido.IdProducto}, con una suma total de: {productoMasVendido.TotalSum}");
Console.WriteLine("-----------------------------------------------------------------------------------");
var local = from t1 in context.Locals join t2 in context.Venta on t1.IdLocal equals t2.IdLocal
            group t2.Total by new { t1.IdLocal, t1.Nombre } into v
            orderby v.Sum() descending
            select new 
            {
               Nombre = v.Key.Nombre,
                total = v.Sum()
            };
var localMas = local.FirstOrDefault();
Console.WriteLine("El local con mayor ventas es: "+localMas.Nombre+" con un monto de: "+localMas.total);
Console.WriteLine("-----------------------------------------------------------------------------------");

var marca = from t1 in context.Marcas
            join t2 in context.Productos on t1.IdMarca equals t2.IdMarca
            join t3 in context.VentaDetalles on t2.IdProducto equals t3.IdProducto
            join t4 in context.Venta on t3.IdVenta equals t4.IdVenta
            group new { t1.Nombre, t2.IdProducto, t4.Total } by new { t1.IdMarca, t1.Nombre, t2.IdProducto } into m
            let totalVenta = m.Sum(x => x.Total)
            let costoTotal = m.Sum(x => x.Total * 0.7) // Ejemplo de cálculo del costo total usando un factor de 0.7 (margen del 30%)
            let margenGanancia = totalVenta - costoTotal
            orderby margenGanancia descending
            select new
            {
                Nombre = m.Key.Nombre,
                IdProducto = m.Key.IdProducto,
                Total = margenGanancia
            };
var marcaMasUtilidad = marca.FirstOrDefault();
Console.WriteLine(marcaMasUtilidad);
Console.WriteLine("-----------------------------------------------------------------------------------");


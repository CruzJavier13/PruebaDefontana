using database_first.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Threading.Tasks.Dataflow;

using (var context = new PruebaContext())
{

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
    var marcaConMayorMargen = (from t1 in context.Venta
                               join t2 in context.VentaDetalles on t1.IdVenta equals t2.IdVenta
                               join t3 in context.Productos on t2.IdProducto equals t3.IdProducto
                               join t4 in context.Marcas on t3.IdMarca equals t4.IdMarca
                               select new
                               {
                                   Marca = t4,
                                   MargenGanancias = (t2.PrecioUnitario - t3.CostoUnitario) * t2.Cantidad
                               })
                               .GroupBy(x => x.Marca)
                               .Select(g => new
                               {
                                   Marca = g.Key,
                                   GananciasTotales = g.Sum(x => x.MargenGanancias)
                               })
                               .OrderByDescending(x => x.GananciasTotales)
                               .FirstOrDefault();



    Console.WriteLine("La marca con mayor margen es: "+marcaConMayorMargen.Marca.Nombre+" con un total de: "+marcaConMayorMargen.GananciasTotales);
    Console.WriteLine("-----------------------------------------------------------------------------------");
    var productosMasVendidosPorLocal = context.VentaDetalles
    .GroupBy(vd => vd.IdVentaNavigation.IdLocal)
    .Select(g => new
    {
        LocalId = g.Key,
        ProductoMasVendido = g.GroupBy(vd => vd.IdProductoNavigation)
            .Select(pg => new
            {
                Producto = pg.Key,
                TotalVentas = pg.Sum(vd => vd.Cantidad)
            })
            .OrderByDescending(pg => pg.TotalVentas)
            .FirstOrDefault()
    })
    .ToList();

    foreach (var l in productosMasVendidosPorLocal)
    {
        var localId = l.LocalId;
        var prodMasVendido = l.ProductoMasVendido;

        Console.WriteLine("Local: "+localId+" Producto mas vendido: "+ prodMasVendido.Producto.Nombre+" Total: "+ prodMasVendido.TotalVentas);
    }
    Console.WriteLine("-----------------------------------------------------------------------------------");
}

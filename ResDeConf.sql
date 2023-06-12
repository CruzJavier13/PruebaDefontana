------------------------------------------------------------------------------------------------
USE Prueba
GO
DECLARE @fechaLimite DATETIME = DATEADD(DAY, -30, GETDATE());
SELECT
    SUM(Total) AS TotalVentas,
    COUNT(*) AS CantidadVentas
FROM
    Venta
WHERE
    Fecha >= @fechaLimite;

SELECT TOP 1
    Fecha,
    Total
FROM
    Venta
WHERE
    Fecha >= @fechaLimite
ORDER BY
    Total DESC;
--------------------------------------------------------------------------------------------------
SELECT
    t1.ID_Producto AS IdProducto,
    SUM(t3.Total) AS TotalSum
FROM
    Producto t1
    JOIN VentaDetalle t2 ON t1.ID_Producto = t2.ID_Producto
    JOIN Venta t3 ON t2.ID_Venta = t3.ID_Venta
GROUP BY
    t1.ID_Producto
ORDER BY
    SUM(t3.Total) DESC;
---------------------------------------------------------------------------------------------------
SELECT
    t1.Nombre AS Nombre,
    SUM(t2.Total) AS Total
FROM
    Local t1
    JOIN Venta t2 ON t1.ID_Local = t2.ID_Local
GROUP BY
    t1.ID_Local,
    t1.Nombre
ORDER BY
    SUM(t2.Total) DESC;
---------------------------------------------------------------------------------------------------
SELECT TOP 1
    t4.Nombre AS Marca,
    SUM((t2.Precio_Unitario - t3.Costo_Unitario) * t2.Cantidad) AS GananciasTotales
FROM
    Venta t1
    JOIN VentaDetalle t2 ON t1.ID_Venta = t2.ID_Venta
    JOIN Producto t3 ON t2.ID_Producto = t3.ID_Producto
    JOIN Marca t4 ON t3.ID_Marca = t4.ID_Marca
GROUP BY
    t4.Nombre
ORDER BY
    SUM((t2.Precio_Unitario - t3.Costo_Unitario) * t2.Cantidad) DESC;
------------------------------------------------------------------------------------------------------
SELECT NombreLocal, NombreProducto, TotalVentas
FROM (
    SELECT l.Nombre AS NombreLocal, p.Nombre AS NombreProducto, SUM(vd.Cantidad) AS TotalVentas,
        ROW_NUMBER() OVER (PARTITION BY l.ID_Local ORDER BY SUM(vd.Cantidad) DESC) AS RowNum
    FROM Local l
    INNER JOIN Venta v ON l.ID_Local = v.ID_Local
    INNER JOIN VentaDetalle vd ON v.ID_Venta = vd.ID_Venta
    INNER JOIN Producto p ON vd.ID_Producto = p.ID_Producto
    GROUP BY l.ID_Local, l.Nombre, p.ID_Producto, p.Nombre
) subquery
WHERE RowNum = 1;
using System;
using System.Collections.Generic;

namespace eShopModel.Classes;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Tipo { get; set; }

    public virtual TipoProducto? TipoNavigation { get; set; } = null!;
}

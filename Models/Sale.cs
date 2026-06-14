using System;
using System.Collections.Generic;

namespace ConsoleApp7;

public partial class Sale
{
    public int SaleId { get; set; }

    public int ItemId { get; set; }

    public int CustomerId { get; set; }

    public int ManagerId { get; set; }

    public int QuantitySold { get; set; }

    public decimal UnitPrice { get; set; }

    public DateOnly SaleDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual StationeryItem Item { get; set; } = null!;

    public virtual Manager Manager { get; set; } = null!;
}

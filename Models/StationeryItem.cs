using System;
using System.Collections.Generic;

namespace ConsoleApp7;

public partial class StationeryItem
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public int TypeId { get; set; }

    public int QuantityInStock { get; set; }

    public decimal CostPrice { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual StationeryType Type { get; set; } = null!;
}

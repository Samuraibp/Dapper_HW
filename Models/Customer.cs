using System;
using System.Collections.Generic;

namespace ConsoleApp7;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CompanyName { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}

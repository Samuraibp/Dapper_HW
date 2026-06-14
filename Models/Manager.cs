using System;
using System.Collections.Generic;

namespace ConsoleApp7;

public partial class Manager
{
    public int ManagerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}

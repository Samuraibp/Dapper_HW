using System;
using System.Collections.Generic;

namespace ConsoleApp7;

public partial class StationeryType
{
    public int TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<StationeryItem> StationeryItems { get; set; } = new List<StationeryItem>();
}

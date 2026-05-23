using System;
using System.Collections.Generic;

namespace Kursovaya_var6.Models;

public partial class Leader
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Band> Bands { get; set; } = new List<Band>();
}

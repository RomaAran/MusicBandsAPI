using System;
using System.Collections.Generic;

namespace Kursovaya_var6.Models;

public partial class Band
{
    public int Id { get; set; }

    public int GroupNameId { get; set; }

    public int LeaderId { get; set; }

    public int AlbumsCount { get; set; }

    public int GenreId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual GroupName GroupName { get; set; } = null!;

    public virtual Leader Leader { get; set; } = null!;
}

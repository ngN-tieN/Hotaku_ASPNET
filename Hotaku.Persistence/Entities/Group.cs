using System;
using System.Collections.Generic;

namespace Hotaku.Persistence.Entities;

public partial class Group
{
    public string GroupId { get; set; } = null!;

    public string? GroupName { get; set; }

    public virtual ICollection<Manga> Mangas { get; set; } = new List<Manga>();
}

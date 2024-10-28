using System;
using System.Collections.Generic;

namespace Hotaku.Persistence.Entities;

public partial class Category
{
    public string CategoryId { get; set; } = null!;

    public string? CategoryName { get; set; }

    public virtual ICollection<Manga> Mangas { get; set; } = new List<Manga>();
}

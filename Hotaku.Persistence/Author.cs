using System;
using System.Collections.Generic;

namespace Hotaku.Persistence;

public partial class Author
{
    public string AuthorId { get; set; } = null!;

    public string? AuthorName { get; set; }

    public string? AuthorBio { get; set; }

    public virtual ICollection<Manga> Mangas { get; set; } = new List<Manga>();
}

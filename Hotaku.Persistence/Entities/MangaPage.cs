using System;
using System.Collections.Generic;

namespace Hotaku.Persistence.Entities;

public partial class MangaPage
{
    public string PageId { get; set; } = null!;

    public string? ChapterId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int? PageNumber { get; set; }

    public virtual MangaChapter? Chapter { get; set; }
}

using System;
using System.Collections.Generic;

namespace Hotaku.Persistence;

public partial class MangaChapter
{
    public string ChapterId { get; set; } = null!;

    public string? MangaId { get; set; }

    public int ChapterNumber { get; set; }

    public string? Title { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Manga? Manga { get; set; }

    public virtual ICollection<MangaPage> MangaPages { get; set; } = new List<MangaPage>();
}

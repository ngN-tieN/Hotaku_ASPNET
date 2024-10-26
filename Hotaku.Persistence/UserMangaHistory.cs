using System;
using System.Collections.Generic;

namespace Hotaku.Persistence;

public partial class UserMangaHistory
{
    public string HistoryId { get; set; } = null!;

    public string? UserId { get; set; }

    public string? MangaId { get; set; }

    public List<string>? ReadChapterIds { get; set; }

    public DateTime? ReadAt { get; set; }

    public virtual Manga? Manga { get; set; }

    public virtual User? User { get; set; }
}

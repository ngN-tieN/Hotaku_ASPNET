using System;
using System.Collections.Generic;

namespace Hotaku.Persistence;

public partial class UserFavoriteManga
{
    public string FavoriteId { get; set; } = null!;

    public string? UserId { get; set; }

    public string? MangaId { get; set; }

    public DateTime? AddedAt { get; set; }

    public virtual Manga? Manga { get; set; }

    public virtual User? User { get; set; }
}

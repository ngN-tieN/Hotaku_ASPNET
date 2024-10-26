using System;
using System.Collections.Generic;

namespace Hotaku.Persistence;

public partial class Manga
{
    public string MangaId { get; set; } = null!;

    public string? Title { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<MangaChapter> MangaChapters { get; set; } = new List<MangaChapter>();

    public virtual ICollection<UserFavoriteManga> UserFavoriteMangas { get; set; } = new List<UserFavoriteManga>();

    public virtual ICollection<UserMangaHistory> UserMangaHistories { get; set; } = new List<UserMangaHistory>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}

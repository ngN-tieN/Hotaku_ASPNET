using System;
using System.Collections.Generic;

namespace Hotaku.Persistence;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public List<int>? Roles { get; set; }

    public virtual ICollection<UserFavoriteManga> UserFavoriteMangas { get; set; } = new List<UserFavoriteManga>();

    public virtual ICollection<UserMangaHistory> UserMangaHistories { get; set; } = new List<UserMangaHistory>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}

using System;
using System.Collections.Generic;

namespace Hotaku.Persistence;

public partial class Notification
{
    public string NotificationId { get; set; } = null!;

    public string? Message { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

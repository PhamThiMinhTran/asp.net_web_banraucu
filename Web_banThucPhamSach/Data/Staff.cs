using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class Staff
{
    public string Id { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string? Address { get; set; }

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? UserName { get; set; }

    public virtual ICollection<MessageChat> MessageChats { get; set; } = new List<MessageChat>();

    public virtual Role? Role { get; set; }
}

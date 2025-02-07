using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class Review
{
    [ValidateNever]
    public string Id { get; set; } = null!;

    public string? ProductId { get; set; }

    public string? UserId { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public int? Rating { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}

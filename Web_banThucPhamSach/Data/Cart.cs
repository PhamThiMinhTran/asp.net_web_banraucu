using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class Cart
{
    public string Id { get; set; } = null!;

    public string? ProductId { get; set; }

    public string? UserId { get; set; }

    public int? Number { get; set; }

    public double? AllTotal { get; set; }

    public int Quantity { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}

using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class OrderDetail
{
    public string Id { get; set; } = null!;

    public string? OrderId { get; set; }

    public string? ProductId { get; set; }

    public double? Price { get; set; }

    public double? Discount { get; set; }

    public int? NumberProducts { get; set; }

    public double? TotalMoney { get; set; }

    public string? PromotionId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Promotion? Promotion { get; set; }
}

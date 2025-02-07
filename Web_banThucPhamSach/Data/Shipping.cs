using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class Shipping
{
    public string Id { get; set; } = null!;

    public string? OrderId { get; set; }

    public string? ShippingMethod { get; set; }

    public string? TrackingNumber { get; set; }

    public string? Status { get; set; }

    public double? ShippingFee { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual Order? Order { get; set; }
}

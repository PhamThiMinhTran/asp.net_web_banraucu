using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class PaymentOption
{
    public int Id { get; set; }

    public string? OrderId { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public string? TransactionId { get; set; }

    public double? TotalAmount { get; set; }

    public virtual Order? Order { get; set; }
}

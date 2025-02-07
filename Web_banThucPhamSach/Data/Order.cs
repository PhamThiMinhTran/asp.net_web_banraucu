using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class Order
{
    public string Id { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Note { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? Status { get; set; }

    public double? TotalMoney { get; set; }

    public string? PaymentMethod { get; set; }

    public string? UserId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();

    public virtual ICollection<PaymentOption> PaymentOptions { get; set; } = new List<PaymentOption>();

    public virtual ICollection<Shipping> Shippings { get; set; } = new List<Shipping>();

    public virtual User? User { get; set; }
}

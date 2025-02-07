using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class Product
{
    public string Id { get; set; } = null!;

    public int? CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public double Price { get; set; }

    public double? Discount { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public string? DescriptionDish { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public int Number { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<Gallery> Galleries { get; set; } = new List<Gallery>();

    public virtual ICollection<InputWarehouse> InputWarehouses { get; set; } = new List<InputWarehouse>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
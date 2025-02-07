using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_banThucPhamSach.Data;

public partial class InputWarehouse
{
    public string Id { get; set; } = null!;

    public string? ProductId { get; set; }

    public string? SuppliersId { get; set; }

    public string? NameProduct { get; set; }

    public int? NumberInput { get; set; }

    public double? Total { get; set; }

    public DateTime? DateIn { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Supplier? Suppliers { get; set; }

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}

public class InputWarehouseViewModel
{
    [Required]
    public string SuppliersId { get; set; } // Nhà cung cấp
    [Required]
    [DataType(DataType.Date)]
    public DateTime DateIn { get; set; } = DateTime.Now;
    public List<ProductInputViewModel> Products { get; set; } = new List<ProductInputViewModel>();
}

public class ProductInputViewModel
{
    public string ProductId { get; set; } // ID sản phẩm
    public string NameProduct { get; set; } // Tên sản phẩm
    public int NumberInput { get; set; } // Số lượng nhập
    public double Price { get; set; } // Giá nhập
    public double Total { get; set; }
}
public class ProductViewModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public double Price { get; set; }
}
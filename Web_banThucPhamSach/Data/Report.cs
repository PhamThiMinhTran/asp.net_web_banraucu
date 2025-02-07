using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class Report
{
    public int Id { get; set; }

    public string ProductId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime ReportDate { get; set; }

    public string ReportType { get; set; } = null!;

    public string? SupplierId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Supplier? Supplier { get; set; }
}
public class ReportViewModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Quarter { get; set; }
    public double TotalRevenue { get; set; }
    public double TotalImportCost { get; set; }
    public double Profit { get; set; }
}
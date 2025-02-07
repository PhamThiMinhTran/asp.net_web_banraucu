using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class Supplier
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<InputWarehouse> InputWarehouses { get; set; } = new List<InputWarehouse>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}

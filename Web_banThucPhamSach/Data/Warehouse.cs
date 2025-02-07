using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class Warehouse
{
    public string Id { get; set; } = null!;

    public string? InputWarehoseId { get; set; }

    public int? NumberProduct { get; set; }

    public virtual InputWarehouse? InputWarehose { get; set; }
}

using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class Gallery
{
    public int Id { get; set; }

    public string? ProductId { get; set; }

    public string? Image { get; set; }

    public virtual Product? Product { get; set; }
}

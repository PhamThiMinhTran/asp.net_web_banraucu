using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class BankAccount
{

    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string AccountType { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [ValidateNever]
    public virtual User User { get; set; } = null!;
}

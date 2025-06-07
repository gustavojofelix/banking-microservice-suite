using AccountService.Domain.Common;

namespace AccountService.Domain.Entities;

public class Account : BaseEntity
{
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;
}

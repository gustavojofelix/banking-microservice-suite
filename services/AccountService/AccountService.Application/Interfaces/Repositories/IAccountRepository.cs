using AccountService.Domain.Entities;

namespace AccountService.Application.Interfaces.Repositories;

public interface IAccountRepository : IGenericRepository<Account>
{
    Task<IEnumerable<Account>> GetAccountsByCustomerIdAsync(Guid customerId);
}

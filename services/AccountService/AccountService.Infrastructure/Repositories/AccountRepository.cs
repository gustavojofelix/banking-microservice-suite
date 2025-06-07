using AccountService.Application.Interfaces.Repositories;
using AccountService.Domain.Entities;
using AccountService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(AccountDbContext context) : base(context) { }

    public async Task<IEnumerable<Account>> GetAccountsByCustomerIdAsync(Guid customerId)
    {
        return await _context.Set<Account>()
            .Where(a => a.CustomerId == customerId)
            .ToListAsync();
    }
}

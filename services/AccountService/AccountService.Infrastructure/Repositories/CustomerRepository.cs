using AccountService.Application.Interfaces.Repositories;
using AccountService.Domain.Entities;
using AccountService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(AccountDbContext context) : base(context) { }

    public async Task<Customer?> GetCustomerWithAccountsAsync(Guid id)
    {
        return await _context.Set<Customer>()
            .Include(c => c.Accounts)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}

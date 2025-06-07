using AccountService.Application.Interfaces;
using AccountService.Application.Interfaces.Repositories;
using AccountService.Infrastructure.Persistence;
using AccountService.Infrastructure.Repositories;

namespace AccountService.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AccountDbContext _context;
    public ICustomerRepository Customers { get; }
    public IAccountRepository Accounts { get; }

    public UnitOfWork(AccountDbContext context)
    {
        _context = context;
        Customers = new CustomerRepository(_context);
        Accounts = new AccountRepository(_context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}

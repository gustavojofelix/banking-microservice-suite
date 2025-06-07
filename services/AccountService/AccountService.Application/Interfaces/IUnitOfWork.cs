using AccountService.Application.Interfaces.Repositories;

namespace AccountService.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICustomerRepository Customers { get; }
    IAccountRepository Accounts { get; }
    Task<int> SaveChangesAsync();
}

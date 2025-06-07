using AccountService.Domain.Entities;

namespace AccountService.Application.Interfaces.Repositories;

public interface ICustomerRepository : IGenericRepository<Customer>
{
    Task<Customer?> GetCustomerWithAccountsAsync(Guid id);
}

using AccountService.Domain.Entities;
using AccountService.Infrastructure.Persistence;
using AccountService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Tests.Repositories;

public class AccountRepositoryTests
{
    private readonly AccountDbContext _context;
    private readonly AccountRepository _repository;

    public AccountRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AccountDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AccountDbContext(options);
        _repository = new AccountRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddAccountToDatabase()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "ACC-001",
            Balance = 1000.00m,
            CustomerId = Guid.NewGuid()
        };

        // Act
        await _repository.AddAsync(account);
        await _context.SaveChangesAsync();

        // Assert
        var saved = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == "ACC-001");
        Assert.NotNull(saved);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectAccount()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "ACC-002",

            Balance = 2000.00m,
            CustomerId = Guid.NewGuid()
        };
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(account.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ACC-002", result!.AccountNumber);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAccounts()
    {
        // Arrange
        _context.Accounts.AddRange(new List<Account>
        {
            new Account { AccountNumber = "ACC-003",  Balance = 3000, CustomerId = Guid.NewGuid() },
            new Account { AccountNumber = "ACC-004",  Balance = 4000, CustomerId = Guid.NewGuid() }
        });
        await _context.SaveChangesAsync();

        // Act
        var all = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, all.Count());
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveAccount()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "ACC-005",

            Balance = 5000.00m,
            CustomerId = Guid.NewGuid()
        };
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        // Act
        _repository.Delete(account);
        await _context.SaveChangesAsync();

        // Assert
        var deleted = await _context.Accounts.FindAsync(account.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task Update_ShouldModifyAccountDetails()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "ACC-101",
            // AccountType = "Savings",
            Balance = 5000,
            CustomerId = Guid.NewGuid()
        };
        await _repository.AddAsync(account);
        await _context.SaveChangesAsync();

        // Act
        account.Balance = 8000;
        //account.AccountType = "Current";
        _repository.Update(account);
        await _context.SaveChangesAsync();

        // Assert
        var updated = await _repository.GetByIdAsync(account.Id);
        Assert.Equal(8000, updated!.Balance);
        //Assert.Equal("Current", updated.AccountType);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNullIfAccountNotFound()
    {
        var id = Guid.NewGuid();
        var result = await _repository.GetByIdAsync(id);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAccountsByCustomerIdAsync_ShouldReturnAccountsForGivenCustomer()
    {
        // Arrange
        var customer = new Customer
        {
            FullName = "Test Customer",
            Email = "test@example.com"
        };

        var unrelatedCustomer = new Customer
        {
            FullName = "Other Customer",
            Email = "other@example.com"
        };

        await _context.Customers.AddRangeAsync(customer, unrelatedCustomer);
        await _context.SaveChangesAsync();

        var accounts = new List<Account>
    {
        new Account
        {
            AccountNumber = "ACC1001",

            Balance = 1000,
            CustomerId = customer.Id
        },
        new Account
        {
            AccountNumber = "ACC1002",

            Balance = 2000,
            CustomerId = customer.Id
        },
        new Account
        {
            AccountNumber = "ACC2001",

            Balance = 3000,
            CustomerId = unrelatedCustomer.Id
        }
    };

        await _context.Accounts.AddRangeAsync(accounts);
        await _context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetAccountsByCustomerIdAsync(customer.Id)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, a => Assert.Equal(customer.Id, a.CustomerId));
        Assert.DoesNotContain(result, a => a.CustomerId == unrelatedCustomer.Id);
    }

    // [Fact]
    // public async Task DeleteAsync_ShouldNotThrowIfAccountNotFound()
    // {
    //     // Arrange
    //     var nonExistentAccount = new Account { Id = Guid.NewGuid() };

    //     // Act & Assert
    //     var ex = await Record.ExceptionAsync(() => _repository.DeleteAsync(nonExistentAccount));
    //     Assert.Null(ex);
    // }

}

using AccountService.Domain.Entities;
using AccountService.Infrastructure.Persistence;
using AccountService.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Tests.Repositories;

public class CustomerRepositoryTests
{
    private readonly AccountDbContext _context;
    private readonly CustomerRepository _repository;

    public CustomerRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AccountDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AccountDbContext(options);
        _repository = new CustomerRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCustomer()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FullName = "Gustavo Felix",
            Email = "gustavojofelix@gmail.com"

        };

        // Act
        await _repository.AddAsync(customer);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);
        result.Should().NotBeNull();
        result!.FullName.Should().Be("Gustavo Felix");
    }

    [Fact]
    public async Task Update_ShouldModifyCustomerDetails()
    {
        // Arrange
        var customer = new Customer { FullName = "Alice", Email = "alice@email.com" };
        await _repository.AddAsync(customer);
        await _context.SaveChangesAsync();

        // Act
        customer.FullName = "Alice Updated";
        customer.Email = "newalice@email.com";
        _repository.Update(customer);
        await _context.SaveChangesAsync();

        // Assert
        var updated = await _repository.GetByIdAsync(customer.Id);
        Assert.Equal("Alice Updated", updated!.FullName);
        Assert.Equal("newalice@email.com", updated.Email);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNullIfCustomerDoesNotExist()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCustomerWithAccountsAsync_ShouldReturnCustomerWithAccounts()
    {
        // Arrange
        var customer = new Customer
        {
            FullName = "Test Customer",
            Email = "customer@example.com"
        };

        var account1 = new Account
        {
            AccountNumber = "ACC001",

            Balance = 1000,
            Customer = customer
        };

        var account2 = new Account
        {
            AccountNumber = "ACC002",

            Balance = 2000,
            Customer = customer
        };

        await _context.Customers.AddAsync(customer);
        await _context.Accounts.AddRangeAsync(account1, account2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetCustomerWithAccountsAsync(customer.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customer.Id, result!.Id);
        Assert.Equal(2, result.Accounts.Count);
        Assert.Contains(result.Accounts, a => a.AccountNumber == "ACC001");
        Assert.Contains(result.Accounts, a => a.AccountNumber == "ACC002");
    }


    // [Fact]
    // public async Task DeleteAsync_ShouldNotThrowIfCustomerDoesNotExist()
    // {
    //     // Arrange
    //     var nonExistentCustomer = new Customer { Id = Guid.NewGuid() };

    //     // Act & Assert
    //     var ex =  Record.ExceptionAsync(() => _repository.Delete(nonExistentCustomer));
    //     Assert.Null(ex);
    // }

}

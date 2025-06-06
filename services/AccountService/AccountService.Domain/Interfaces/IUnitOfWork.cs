namespace AccountService.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}

namespace DavidVDom.Domain.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

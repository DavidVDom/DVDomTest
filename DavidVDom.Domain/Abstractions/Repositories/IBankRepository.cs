using DavidVDom.Domain.Entities;

namespace DavidVDom.Domain.Abstractions.Repositories
{
    public interface IBankRepository
    {
        Task<IEnumerable<Bank>> GetAll(CancellationToken cancellationToken = default);
        Task<Bank?> GetById(Guid id, CancellationToken cancellationToken = default);
        void Add(Bank bank);
    }
}

using DavidVDom.Domain.Abstractions;

namespace DavidVDom.Infraestructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DavidVDomDbContext _dbContext;

        public UnitOfWork(DavidVDomDbContext dbContext) => _dbContext = dbContext;

        public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _dbContext.SaveChangesAsync(cancellationToken);

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}

using DavidVDom.Domain.Abstractions.Repositories;
using DavidVDom.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DavidVDom.Infraestructure.Repositories
{
    public class BanksRepository : IBankRepository
    {
        private readonly DavidVDomDbContext _dbContext;

        public BanksRepository(DavidVDomDbContext dbContext) => _dbContext = dbContext;

        public void Add(Bank bank)
        {
            _dbContext.Banks.Add(bank);
        }

        public async Task<IEnumerable<Bank>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Banks.ToListAsync(cancellationToken);
        }

        public async Task<Bank?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Banks.Where(b => b.Id == id).FirstOrDefaultAsync(cancellationToken);
        }
    }
}

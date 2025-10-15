using HospitalManagementSystem.Core;
using HospitalManagementSystem.Core.Repository.Contracts;
using HospitalManagementSystem.Repository.Data;
using System.Collections;

namespace HospitalManagementSystem.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HospitalManagementContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(HospitalManagementContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var key = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(key))
            {
                var respository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(key, respository);
            }
            return _repositories[key] as IGenericRepository<TEntity>;
        }
        public Task<int> CompleteAsync()
            => _dbContext.SaveChangesAsync();

        public ValueTask DisposeAsync()
            => _dbContext.DisposeAsync();


    }
}

using Ardalis.Specification;
using ASP.Net_Core_3._0_Web_API.ApplicationCore.Entities;
using ASP.Net_Core_3._0_Web_API.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.Infraestructure.Data
{
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {

        #region Fields

        protected CatalogContext Context;

        #endregion

        public EfRepository(CatalogContext context)
        {
            Context = context;
        }

        #region Public Methods

        public Task<T> GetById(int id) => Context.Set<T>().FindAsync(id).AsTask();

        public Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
            => Context.Set<T>().FirstOrDefaultAsync(predicate);

        public async Task Add(T entity)
        {
            // await Context.AddAsync(entity);
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public Task Update(T entity)
        {
            // In case AsNoTracking is used
            Context.Entry(entity).State = EntityState.Modified;
            return Context.SaveChangesAsync();
        }

        public Task Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
            return Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> List(ISpecification<T> spec)
        {
            var specificationResult = await ApplySpecification(spec);
            return await specificationResult.ToListAsync();
        }

        private async Task<IQueryable<T>> ApplySpecification(ISpecification<T> spec)
        {
            return await EfSpecificationEvaluator<T>.GetQuery(Context.Set<T>().AsQueryable(), spec);
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().Where(predicate).ToListAsync();
        }

        public Task<int> CountAll() => Context.Set<T>().CountAsync();

        public Task<int> CountWhere(Expression<Func<T, bool>> predicate)
            => Context.Set<T>().CountAsync(predicate);

        #endregion

    }
}

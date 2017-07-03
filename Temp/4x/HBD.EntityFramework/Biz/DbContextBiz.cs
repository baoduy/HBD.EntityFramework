using HBD.EntityFramework.Validation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Threading.Tasks;

namespace HBD.EntityFramework.Biz
{
    /// <summary>
    /// This DbContextBiz will be implement using Async technical to make it compatible with Service.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    [ServiceContract]
    public abstract class DbContextBiz<TEntity> : IBiz<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;

        protected DbContextBiz(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        protected virtual IEnumerable<Expression<Func<TEntity, object>>> GetIncludePath() => null;

        /// <summary>
        /// GetQuery with Includes Properties
        /// </summary>
        /// <returns>IQueryable</returns>
        protected IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = this._dbContext.Set<TEntity>().AsQueryable();
            var includePath = this.GetIncludePath();

            if (includePath != null)
            {
                query = includePath.Where(include => include != null)
                    .Aggregate(query, (current, include) => current.Include(include));
            }

            if (predicate != null)
                query = query.Where(predicate);
            return query;
        }

        #region Default Non-Async Methods

        public int Add(params TEntity[] entities)
        {
            this.Validation(ValidateState.Added, entities.ToArray());
            this._dbContext.Set<TEntity>().AddRange(entities);
            return this._dbContext.SaveChanges();
        }

        public int Delete(params TEntity[] entities)
        {
            this.Validation(ValidateState.Deleted, entities.ToArray());
            this._dbContext.Set<TEntity>().RemoveRange(entities);
            return this._dbContext.SaveChanges();
        }

        public int Update(params TEntity[] entities)
        {
            this.Validation(ValidateState.Modified, entities.ToArray());

            foreach (var t in entities)
            {
                var entry = this._dbContext.Entry(t);

                if (entry != null)
                    entry.State = EntityState.Modified;
            }

            return this._dbContext.SaveChanges();
        }

        public IList<TEntity> GetAll() => GetQueryable().ToList();

        public TEntity GetById(params object[] keys) => this._dbContext.Set<TEntity>().Find(keys);

        #endregion Default Non-Async Methods

        #region Default Async Methods

        public Task<int> AddAsync(params TEntity[] entities)
        {
            this.Validation(ValidateState.Added, entities.ToArray());
            this._dbContext.Set<TEntity>().AddRange(entities);
            return this._dbContext.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(params TEntity[] entities)
        {
            this.Validation(ValidateState.Modified, entities.ToArray());
            foreach (var t in entities)
            {
                var entry = this._dbContext.Entry(t);

                if (entry != null)
                    entry.State = EntityState.Modified;
            }

            return this._dbContext.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(params TEntity[] entities)
        {
            this.Validation(ValidateState.Deleted, entities.ToArray());
            this._dbContext.Set<TEntity>().RemoveRange(entities);
            return this._dbContext.SaveChangesAsync();
        }

        public Task<List<TEntity>> GetAllAsync() => GetQueryable().ToListAsync();

        public Task<TEntity> GetByIdAsync(params object[] keys) => this._dbContext.Set<TEntity>().FindAsync(keys);

        #endregion Default Async Methods

        private void Validation(ValidateState state, params TEntity[] entities)
        {
            var list = (from e in entities select new ValidationResult<TEntity>(e, state)).ToList();

            this.ValidateEntities(list);

            var invalidEntities = list.Where(e => !e.IsValid).ToList();
            if (invalidEntities.Any())
                throw new ValidationException<TEntity>(invalidEntities);
        }

        protected virtual void ValidateEntities(IList<ValidationResult<TEntity>> entities)
        {
        }
    }
}
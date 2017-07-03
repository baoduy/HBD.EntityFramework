#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HBD.EntityFramework.Core;

#endregion

namespace HBD.EntityFramework.Repositories
{
    public interface IRepository
    {
    }

    public interface IRepository<TEntity> : IRepository where TEntity : class, IEntity
    {
        IQueryable<TEntity> AsQueryable(Expression<Func<TEntity, bool>> predicate = null);

        IOrderedQueryable<TEntity> AsOrderable<TKey>(Expression<Func<TEntity, TKey>> orderBy,
            OrderDirection direction = OrderDirection.Asscending, Expression<Func<TEntity, bool>> predicate = null);

        IOrderedQueryable<TEntity> AsOrderable(string orderBy,
            OrderDirection direction = OrderDirection.Asscending, Expression<Func<TEntity, bool>> predicate = null);

        TEntity Find(params object[] keyValues);
        Task<TEntity> FindAsync(params object[] keyValues);

        long Count(Expression<Func<TEntity, bool>> predicate = null);
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate = null);

        IEnumerable<TEntity> All(Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, object>> orderBy = null, OrderDirection direction = OrderDirection.Asscending);

        Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, object>> orderBy = null, OrderDirection direction = OrderDirection.Asscending);

        IPagable<TEntity> Page<TKey>(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> orderBy, OrderDirection direction = OrderDirection.Asscending,
            Expression<Func<TEntity, bool>> predicate = null);

        Task<IPagable<TEntity>> PageAsync<TKey>(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> orderBy, OrderDirection direction = OrderDirection.Asscending,
            Expression<Func<TEntity, bool>> predicate = null);


        /// <summary>
        ///     Begins tracking the given entity, and any other reachable entities that are not
        ///     already being tracked, in the Microsoft.EntityFrameworkCore.EntityState.Added
        ///     state such that they will be inserted into the database when IRepositoryFactory.SaveChanges
        ///     is called.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="creatingUser"></param>
        void Add(TEntity item, string creatingUser);

        void AddRange(IEnumerable<TEntity> items, string creatingUser);

#if NETSTANDARD2_0 || NETSTANDARD1_6
        /// <summary>
        ///     Begins tracking the given entity, and any other reachable entities that are not
        ///     already being tracked, in the Microsoft.EntityFrameworkCore.EntityState.Added
        ///     state such that they will be inserted into the database when IRepositoryFactory.SaveChanges
        ///     is called.
        ///     This method is async only to allow special value generators.
        ///     to access the database asynchronously. For all other cases the non async method
        ///     should be used.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task AddAsync(TEntity item, string userName);

        Task AddRangeAsync(IEnumerable<TEntity> items, string creatingUser);
#endif

        /// <summary>
        ///     Update entity in context without save to data base.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="updatingUser"></param>
        void Update(TEntity item, string updatingUser);

        void UpdateRange(IEnumerable<TEntity> items, string updatingUser);

        /// <summary>
        ///     Delete entity from context without save to data base.
        /// </summary>
        /// <param name="item"></param>
        void Delete(TEntity item, string deletingUser);

        void DeleteRange(IEnumerable<TEntity> items, string deletingUser);

        /// <summary>
        ///     Delete entity by key values from context without save to data base.
        /// </summary>
        /// <param name="keyValues"></param>
        void DeleteByKey(params object[] keyValues);
        void DeleteAll(string deletingUser);

        /// <summary>
        ///     Delete entity by key values from context without save to data base.
        /// </summary>
        /// <param name="keyValues"></param>
        Task DeleteByKeyAsync(params object[] keyValues);

        /// <summary>
        ///     Delete those items in db when it satisfy the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        void DeleteWhere(Expression<Func<TEntity, bool>> predicate, string deletingUser);
    }
}
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace HBD.EntityFramework.Biz
{
    [ServiceContract]
    public interface IBiz<TEntity> where TEntity : class
    {
        #region Default Non-Async Methods

        int Add(params TEntity[] entities);

        int Update(params TEntity[] entities);

        int Delete(params TEntity[] entities);

        IList<TEntity> GetAll();

        TEntity GetById(params object[] keys);

        #endregion Default Non-Async Methods

        #region Default Async Methods

        [OperationContract]
        Task<int> AddAsync(params TEntity[] entities);

        [OperationContract]
        Task<int> UpdateAsync(params TEntity[] entities);

        [OperationContract]
        Task<int> DeleteAsync(params TEntity[] entities);

        [OperationContract]
        Task<List<TEntity>> GetAllAsync();

        [OperationContract]
        Task<TEntity> GetByIdAsync(params object[] keys);

        #endregion Default Async Methods
    }
}
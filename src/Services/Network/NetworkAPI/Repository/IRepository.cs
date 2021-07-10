using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using NetworkAPI.Dtos;
using NetworkAPI.Utils;

namespace NetworkAPI.Repository
{
    public interface IRepository<TModel>
        where TModel: class
    {
        Task<IQueryable<TModel>> Find(Expression<Func<TModel, bool>> expression);
        Task<TModel> GetByIdAsync(Guid id);
        Task<PagedList<TModel>> GetAllAsync(CancellationToken cancellationToken, PaginationDto paginationDto);

        Task<IEnumerable<TModel>> GetByListOfIdsAsync(IEnumerable<Guid> ids);

        Task<bool> SaveChangesAsync();

        Task<Response<TModel>> CreateAsync(TModel model);

        void UpdateAsync(TModel model);
        void Delete(TModel model);

        Task<int> GetCountAsync();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DeviceAPI.Dtos;
using DeviceAPI.Models;
using DeviceAPI.Utils;

namespace DeviceAPI.Repository
{
    public class BaseRepo<TModel> : IRepository<TModel>
        where TModel : BaseModel
    {
        protected readonly DeviceContext _context;
        private readonly DbSet<TModel> _table;

        public BaseRepo(DeviceContext context)
        {
            _context = context;
            _table = _context.Set<TModel>();
        }


        public Task<IQueryable<TModel>> Find(Expression<Func<TModel, bool>> expression)
        {
            throw new NotImplementedException();
        } 

        public virtual async Task<TModel> GetByIdAsync(Guid id)
        {
            return await _table.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken)
        {
            PagedList<TModel> pagedList = await PagedList<TModel>.ToPagedList(_table, 1, 5, cancellationToken);
            return pagedList.Entities;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<Response<TModel>> CreateAsync(TModel model)
        {
            await _table.AddAsync(model);
            return new Response<TModel>(model);
        }

        public void UpdateAsync(TModel model)
        {
            //noop
        }

        public void DeleteAsync(TModel model)
        {
            _table.Remove(model);
        }

        public async Task<int> GetCountAsync()
        {
            return await _table.CountAsync();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using OnlineCoachingSystem.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCoachingSystem.EF.Implementation
{
    public class BaseRepository<Entity> : IBaseRepository<Entity> where Entity : class
    {
        protected ApplicationDbContext _DbContext;

        internal DbSet<Entity> MyDbSet;
        public BaseRepository(ApplicationDbContext DbContext) 
        { 
            _DbContext = DbContext;
            MyDbSet = _DbContext.Set<Entity>();
        } 
        

        async Task<IEnumerable<Entity>> IBaseRepository<Entity>.AddRangeAsync(IEnumerable<Entity> entities)
        {
            await MyDbSet.AddRangeAsync(entities);
            return entities;
        }

          void IBaseRepository<Entity>.AttachRange(IEnumerable<Entity> entities)
        {
             MyDbSet.AttachRange(entities);   
        }

        public async Task<int> IBaseRepository<Entity>.CountAsync(Expression<Func<Entity, bool>>? criteria)
        {
            return await MyDbSet.CountAsync(criteria);
        }

        void IBaseRepository<Entity>.Delete(Entity entity)
        {
            throw new NotImplementedException();
        }

        void IBaseRepository<Entity>.DeleteRange(IEnumerable<Entity> entities)
        {
            throw new NotImplementedException();
        }

        Entity IBaseRepository<Entity>.Find(Expression<Func<Entity, bool>> criteria, string[] includes)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Entity> IBaseRepository<Entity>.FindAll(Expression<Func<Entity, bool>> criteria, string[] includes)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Entity> IBaseRepository<Entity>.FindAll(Expression<Func<Entity, bool>> criteria, int take, int skip)
        {
            throw new NotImplementedException();
        }

        //IEnumerable<Entity> IBaseRepository<Entity>.FindAll(Expression<Func<Entity, bool>> criteria, int? take, int? skip, Expression<Func<Entity, object>> orderBy, string orderByDirection)
        //{

        //}

        Task<IEnumerable<Entity>> IBaseRepository<Entity>.FindAllAsync(Expression<Func<Entity, bool>> criteria, string[] includes)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Entity>> IBaseRepository<Entity>.FindAllAsync(Expression<Func<Entity, bool>> criteria, int skip, int take)
        {
            throw new NotImplementedException();
        }

        //Task<IEnumerable<Entity>> IBaseRepository<Entity>.FindAllAsync(Expression<Func<Entity, bool>> criteria, int? skip, int? take, Expression<Func<Entity, object>> orderBy, string orderByDirection)
        //{
        //    throw new NotImplementedException();
        //}

        Task<Entity> IBaseRepository<Entity>.FindAsync(Expression<Func<Entity, bool>> criteria, string[] includes)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Entity> IBaseRepository<Entity>.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Entity>> IBaseRepository<Entity>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Entity IBaseRepository<Entity>.GetById(int id)
        {
            throw new NotImplementedException();
        }

        Task<Entity> IBaseRepository<Entity>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Entity IBaseRepository<Entity>.Update(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}

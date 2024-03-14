using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using OnlineCoachingSystem.Repository;
using OnlineCoachingSystem.Repository.IRepositories;

using System.Collections.Generic;

using  System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCoachingSystem.EF.Implementation
{
    public class BaseRepository<Entity,Dto> :IBaseRepository<Entity,Dto> 
        where Entity : class
        where Dto : class
    {
        protected ApplicationDbContext _DbContext;

        private readonly IMapper _mapper;
        internal DbSet<Entity> MyDbSet;
        public BaseRepository(ApplicationDbContext DbContext,IMapper Mapper) 
        { 
            _DbContext = DbContext;
            _mapper = Mapper;
            MyDbSet = _DbContext.Set<Entity>();
        }

        public async Task<IEnumerable<Entity>> AsyncAddRange(IEnumerable<Dto> Dtos)
        {
            
            IEnumerable<Entity> entities = _mapper.Map<IEnumerable<Entity>>(Dtos);   
            await MyDbSet.AddRangeAsync(entities);
            return entities;
        }

        public async Task<Entity> AsyncAdd(IEnumerable<Dto> Dto)
        {
            Entity entity=_mapper.Map<Entity>(Dto);
            await MyDbSet.AddAsync(entity);
            return entity;
        }
        public void RangeAttack(IEnumerable<Dto> Dtos)
        {
            IEnumerable<Entity> entities = _mapper.Map<IEnumerable<Entity>>(Dtos);
            MyDbSet.AttachRange(entities);   
        }
        public async Task<int>  AsyncCount(Expression<Func<Entity, bool>>? criteria)
        {
            return await MyDbSet.CountAsync(criteria);
        }

        public void DeleteConditionRangeAsync( Expression<Func<Entity, bool>>? criteria)
        {
            IQueryable<Entity> query;
            if (criteria != null)
                query = MyDbSet.Where(criteria);
            else
                query = MyDbSet;
            query.ExecuteDeleteAsync();
        }
        public  void DeleteRange(IEnumerable<Dto> Dtos)
        {
            IEnumerable<Entity> entities = _mapper.Map<IEnumerable<Entity>>(Dtos);
            MyDbSet.RemoveRange(entities);
        }

        public async Task<IEnumerable<Entity>> FindAllAsync(Expression<Func<Entity, bool>>? criteria, int? take, int? skip,
        Expression<Func<Entity , object>> orderBy = null
            , string orderByDirection = MagicStrings.Ascending)
        {
            IQueryable<Entity> query;
            if (criteria !=null)
                     query = MyDbSet.Where(criteria);
            else
                     query = MyDbSet;

            if (take.HasValue)
                query = query.Take(take.Value);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (orderBy != null)
            {
                if (orderByDirection == MagicStrings.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }

        public async Task<Entity> FindEntityAsync(Dto dto)
        {

            var entity = _mapper.Map<Entity>(dto);
            return await MyDbSet.FindAsync(entity);
        }
        public async Task<IEnumerable<Entity>> GetAllAsync()
        { 
            return await MyDbSet.ToListAsync(); 
        }


        public async Task<Entity> GetByIdAsync(int id)
        {
            return await MyDbSet.FindAsync(id);
        }

        public  IEnumerable<Entity> AsyncUpdateRange(IEnumerable<Dto> Dtos)
        {
            IEnumerable<Entity> entities = _mapper.Map<IEnumerable<Entity>>(Dtos);
            MyDbSet.UpdateRange(entities);
            return  entities;
        }

    }
}

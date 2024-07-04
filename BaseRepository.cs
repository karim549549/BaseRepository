using Azure;
using CollageManagementSystem.BussniessLogicLayer.Dtos.StudentDtos;
using CollageManagementSystem.BussniessLogicLayer.Helpers;
using CollageManagmentSystem.businessLogicLayer.MagicData;
using CollageManagmentSystem.businessLogicLayer.Models;
using CollageManagmentSystem.businessLogicLayer.Services.IRepositories;
using CollageManagmentSystem.DataAccesslayer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;

using System.Data.Entity.Infrastructure;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CollageManagementSystem.DataAccessLayer.Repositories
{
    public class BaseRepository<Entity> : IBaseRepository<Entity>
        where Entity : class
    {
        private readonly ApplicationDbContext _context;
        protected DbSet<Entity> _dbSet;
         
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Entity>();
        }
        public BaseRepository(){}

        public async Task<ApiResponse> AddRangeAsync(IEnumerable<Entity> entity)
        {
            await _dbSet.AddRangeAsync(entity);
            var response = await ChangesSaved();
            if (!response.Success)
            {
                return response.Response;
            }
            return  new ApiResponse
            {
                Data=entity,
                Status=HttpStatusCode.OK,
                IsSuccess=true
            };
        }

        public async Task<ApiResponse> Delete(Expression<Func<Entity, bool>> expression)
        {
        
            var query = _dbSet.Where(expression);
            var queryToList=await query.ToListAsync();
            var result = await query.ExecuteDeleteAsync();
            if(result == 0)
            {
                new ApiResponse
                {
                    Message = MagicLists.DatabaseError,
                    Status = HttpStatusCode.BadRequest,
                    IsSuccess = false
                };
            }
            return new ApiResponse
            {
                Data= queryToList,
                Status=HttpStatusCode.OK,
                IsSuccess=true
            };

        }

        public async Task<ApiResponse> GetAll<Dto>
            (
            bool toDto,
            string[]? include = null,
            Expression<Func<Entity, bool>>? condition = null,
            int? pageSize = null,
            int? pageNumber = null
            )
        {
            IQueryable<Entity> query = _dbSet;

            if (condition != null)
            {
                query = query.Where(condition);
            }


            if (include != null)
            {
                foreach (var includeProperty in include)
                {
                    query = query.Include(includeProperty);
                }
            }

            int totalCount = await query.CountAsync();
            PaginationMetadata paginationMetadata = new PaginationMetadata();

    
            if (pageSize.HasValue && pageNumber.HasValue)
            {
                paginationMetadata.TotalCount = totalCount;
                paginationMetadata.PageSize = pageSize.Value;
                paginationMetadata.CurrentPage = pageNumber.Value;
                paginationMetadata.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize.Value);

                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            if (toDto)
            {
                return new ApiResponse
                {
                    Data = await query.ProjectToType<Dto>().ToListAsync(),
                    Status = HttpStatusCode.OK,
                    IsSuccess = true,
                    PaginationMetadata = paginationMetadata
                };
            }
            else
            {
                return new ApiResponse
                {
                    Data = query.ToListAsync(),
                    Status = HttpStatusCode.OK,
                    IsSuccess = true,
                    PaginationMetadata = paginationMetadata
                };
            }
        }
        public async Task<ApiResponse> Update(Expression <Func<Entity,bool>> Condition,Entity entity)
        {
            var Course =  await _dbSet.Where(Condition).FirstOrDefaultAsync();
            _dbSet.Entry(Course).CurrentValues.SetValues(entity);
            var response = await ChangesSaved();
            if (!response.Success)
            {
                return response.Response;
            }
            return new ApiResponse
            {
                Data = entity,
                Status = HttpStatusCode.OK,
                IsSuccess = true
            };
        }
        private async Task<OperationResponse> ChangesSaved()
        {
            var result= await _context.SaveChangesAsync();
            if(result == 0)
            {
                return new OperationResponse
                {   
                    Response=new ApiResponse
                    {
                        Message=MagicLists.DatabaseError,
                        Status=HttpStatusCode.BadRequest,
                        IsSuccess=false
                    },
                    Success=false
                };
            }
            return new OperationResponse
            {
                Success = true
            };
        }
    }
}


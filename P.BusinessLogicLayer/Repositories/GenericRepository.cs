using Microsoft.EntityFrameworkCore;
using P.BusinessLogicLayer.Interfaces;
using P.DataAccessLayer.Contexts;
using P.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.BusinessLogicLayer.Repositories
{
    public  class GenericRepository<T>:IGenericRepository<T> where T:class
    {
        private readonly MvcAppDbcontext _dbcontext;
        public GenericRepository(MvcAppDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddAsync(T item)
        {
            await _dbcontext.AddAsync(item);
            
        }

        public void Delete(T item)
        {
            _dbcontext.Remove(item);
           
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T)==typeof(Employee))
            {
                return (IEnumerable<T>)await _dbcontext.Employees.Include(E=>E.Department).ToListAsync();
            }
            return await _dbcontext.Set<T>().ToListAsync();    
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbcontext.Set<T>().FindAsync(id);
        }

        public void Update(T item)
        {
            _dbcontext.Update(item);
            
        }
    }
}

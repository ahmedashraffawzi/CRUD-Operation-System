using P.BusinessLogicLayer.Interfaces;
using P.DataAccessLayer.Contexts;
using P.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P.BusinessLogicLayer.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MvcAppDbcontext _dbcontext;
        public EmployeeRepository(MvcAppDbcontext dbcontext) : base(dbcontext)
        {

        }
        public IQueryable<Employee> GetEmployeeByAddress(string address)
        
            =>_dbcontext.Employees.Where(E => E.Address == address);

        public IQueryable<Employee> SearchEmployeesByName(string name)
        {
            return _dbcontext.Employees.Where(E=>E.Name.ToLower().Contains(name.ToLower()));
        }
    }
}

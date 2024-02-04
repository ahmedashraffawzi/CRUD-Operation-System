using P.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.BusinessLogicLayer.Interfaces
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
        IQueryable<Employee>GetEmployeeByAddress(string address);
        IQueryable<Employee> SearchEmployeesByName(string name);
    }
}

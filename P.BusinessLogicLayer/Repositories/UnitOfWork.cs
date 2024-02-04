using P.BusinessLogicLayer.Interfaces;
using P.DataAccessLayer.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.BusinessLogicLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly MvcAppDbcontext _dbcontext;
        public IEmployeeRepository EmployeeRepository { get ; set ; }
        public IDepartmentRepository DepartmentRepository { get; set; }
        public UnitOfWork(MvcAppDbcontext dbcontext)
        {
            EmployeeRepository=new EmployeeRepository(dbcontext);
            DepartmentRepository=new DepartmentRepository(dbcontext);
            _dbcontext=dbcontext;
        }

        public async Task<int> CompleteAsync()
        
            =>await _dbcontext.SaveChangesAsync();

        public void Dispose()
        {
            _dbcontext.Dispose();
        }
    }
}

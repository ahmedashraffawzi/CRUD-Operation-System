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
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(MvcAppDbcontext dbcontext):base(dbcontext)
        {
            
        }
    }
}

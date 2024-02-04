using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using P.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.DataAccessLayer.Contexts
{
     public class MvcAppDbcontext:IdentityDbContext<ApplicationUser>
    {
        public MvcAppDbcontext(DbContextOptions<MvcAppDbcontext> options) : base(options) 
        {
                    
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // => optionsBuilder.UseSqlServer("Server=.;Database=MvcApp;Trusted_Connection=true");


        public DbSet<Department>Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

         
    }
}

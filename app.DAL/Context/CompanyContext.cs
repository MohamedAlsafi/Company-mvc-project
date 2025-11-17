using app.DAL.model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.Context
{
	public class CompanyContext: IdentityDbContext <User>
	{
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //	optionsBuilder.UseSqlServer("server =.;database=mvcApp;trusred_connection=true ");	
        //}
        public CompanyContext(DbContextOptions<CompanyContext> options ): base(options) { }
        
        public DbSet <Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

    }
}

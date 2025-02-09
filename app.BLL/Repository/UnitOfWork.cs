using app.BLL.Interface;
using app.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.BLL.Repository
{
    public class UnitOfWork : IUnitOfWork , IDisposable
    {
        private readonly CompanyContext _context;

        public IDepartmentRepository departmentRepository { get ; set ; }
        public IEmployeeReopsitory employeeReopsitory { get ; set ; }
        public async Task<int> Complete()
        
          => await _context.SaveChangesAsync();

        public void Dispose()
        
       => _context.Dispose();
        

        public UnitOfWork(CompanyContext context)
        {
            employeeReopsitory = new EmployeeReopsitory(context);
            departmentRepository = new DepartmentRepository(context);
            _context = context;
        }

    }
}

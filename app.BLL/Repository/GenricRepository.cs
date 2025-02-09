using app.BLL.Interface;
using app.DAL.Context;
using app.DAL.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.BLL.Repository
{
    public class GenricRepository<T> : IGenricRepository<T> where T : class
    {
        private readonly CompanyContext _context;

        public GenricRepository(CompanyContext context)
        {
            _context = context;
        }
        public async Task Add(T item)
        {
           await _context.Set<T>().AddAsync(item);
        }

        public void Delete(T item)
        {
            _context.Set<T>().Remove(item);
        }

        public async Task<T> Get(int id)
       => await _context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> GetAll()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(E => E.Department).ToListAsync();
            }else
              return await _context.Set<T>().ToListAsync();
        }
        

        public void Update(T item)
        {
            _context.Set<T>().Update(item);
        }
    }
}

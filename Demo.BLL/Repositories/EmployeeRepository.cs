using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MVCAppDbContext _context;
        public EmployeeRepository(MVCAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentName(string DeptName)
            => await _context.Employees.Where(e => e.Department.Name == DeptName).ToListAsync();

        public async Task<string> GetDepartmentByEmployeeId(int? id)
        {
           var employee =  await _context.Employees.Where(e => e.Id == id).Include(e => e.Department).FirstOrDefaultAsync();
            var department = employee.Department;
            return department.Name;
        }

        public async Task<IEnumerable<Employee>> Search(string name)
             => await _context.Employees.Where(e => e.Name.Contains(name)).ToListAsync();
    }
}

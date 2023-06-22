using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {

        //ReadOnly Property => It cannot be modified in Run Time.
        private readonly MVCAppDbContext _context;
        public DepartmentRepository(MVCAppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

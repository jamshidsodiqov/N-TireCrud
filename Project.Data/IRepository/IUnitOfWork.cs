using Project.Domain.Entities.Courses;
using Project.Domain.Entities.Students;
using Project.Domain.Entities.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.IRepository
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Course> Courses { get;  }
        public IGenericRepository<Student> Students { get; }
        public IGenericRepository<Teacher> Teachers { get; }

        ValueTask SaveChangesAsync();
    }
}

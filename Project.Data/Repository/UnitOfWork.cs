using Project.Data.DbContexts;
using Project.Data.IRepository;
using Project.Domain.Entities.Courses;
using Project.Domain.Entities.Students;
using Project.Domain.Entities.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        public readonly AppDbContext dbContext;

        public UnitOfWork()  
        {
            dbContext = new AppDbContext();
            Courses = new GenericRepository<Course>();
            Students = new GenericRepository<Student>();
            Teachers = new GenericRepository<Teacher>(); 
        }

        public IGenericRepository<Course> Courses { get; }
        public IGenericRepository<Student> Students { get; }
        public IGenericRepository<Teacher> Teachers { get; }

        public async ValueTask SaveChangesAsync()
            => await dbContext.SaveChangesAsync();


    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Data.DbContexts;
using Project.Data.IRepository;
using Project.Data.Repository;
using Project.Domain.Configuration;
using Project.Domain.Entities.Students;
using Project.Domain.Entities.Teachers;
using Project.Service.DTOs.Students;
using Project.Service.DTOs.Teachers;
using Project.Service.Exceptions;
using Project.Service.Extensions;
using Project.Service.Interfaces;
using Project.Service.Mappers;
using System.Linq.Expressions;

namespace Project.Service.Services
{
    public class TeacherService : ITeacherService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TeacherService()
        {
            unitOfWork = new UnitOfWork();
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }).CreateMapper();
        }

        public async ValueTask<TeacherForViewDTO> CreateAsync(TeacherForCreationDTO TeacherForCreationDTO)
        {
            var existTeacher = await unitOfWork.Teachers.GetAsync(e => e.Email == TeacherForCreationDTO.Email);

            if (existTeacher != null)
            {
                throw new ProjectException("This teacher created already");
            }
            var createdTeacher = await unitOfWork.Teachers.CreateAsync(mapper.Map<Teacher>(TeacherForCreationDTO));
            await unitOfWork.SaveChangesAsync();

            return mapper.Map<TeacherForViewDTO>(createdTeacher);
        }

        public async ValueTask<bool> DeleteAsync(int id)
        {
            var deletedteacher = await unitOfWork.Teachers.DeleteAsync(id);

            if (!deletedteacher)
            {
                throw new ProjectException("Teacher not found");
            }

            await unitOfWork.SaveChangesAsync();

            return true;
        }

        public async ValueTask<IEnumerable<TeacherForViewDTO>> GetAllAsync(PaginationParams @params, Expression<Func<Teacher, bool>> expression = null)
        {
            var teachers = unitOfWork.Teachers.GetAll(expression, isTracking: false);

            return mapper.Map<IEnumerable<TeacherForViewDTO>>(await teachers.ToPagedList(@params).ToListAsync());
        }

        public async ValueTask<TeacherForViewDTO> GetAsync(Expression<Func<Teacher, bool>> expression)
        {
            var teacher = unitOfWork.Teachers.GetAsync(expression, isTracking: false);

            if (teacher == null)
                throw new ProjectException("Teacher not found");

            return mapper.Map<TeacherForViewDTO>(teacher);
        }

        public async ValueTask<TeacherForViewDTO> UpdateAsync(int id, TeacherForUpdateDTO TeacherForUpdateDTO)
        {
            var existTeacher = await unitOfWork.Teachers.GetAsync(s => s.Id == id);

            if (existTeacher == null)
                throw new ProjectException("404 Teacher not found");

            var Teacher = await unitOfWork.Teachers.GetAsync(s => s.Email == TeacherForUpdateDTO.Email && s.Id != id);

            if (Teacher != null)
                throw new ProjectException("This student already exist");

            existTeacher = unitOfWork.Teachers.UpdateAsync(mapper.Map<Teacher>(TeacherForUpdateDTO));

            await unitOfWork.SaveChangesAsync();

            return mapper.Map<TeacherForViewDTO>(existTeacher);
        }
    }
}
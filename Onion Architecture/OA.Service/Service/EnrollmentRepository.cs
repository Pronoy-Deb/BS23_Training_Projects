using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OA.Data.Model;
using OA.Repo.Data;
using OA.Repo.Repository;
using OA.Service.Service;
using Microsoft.EntityFrameworkCore;

namespace OA.Service.Service
{
    public class EnrollmentRepository : BaseRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(AppDbContext context) : base(context) { }

        private bool EnrollmentExistsAsync(int courseId, int studentId)
        {
            return _dbSet.Any(e => e.CourseId == courseId && e.StudentId == studentId);
        }

        public async Task<IEnumerable<Enrollment>> GetCourseEnrollmentsAsync(int courseId)
        {
            return await _dbSet
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Where(e => e.CourseId == courseId)
                .ToListAsync();
        }

        public override async Task AddAsync(Enrollment entity)
        {
            if (EnrollmentExistsAsync(entity.CourseId, entity.StudentId))
                throw new Exception($"Student with Id:{entity.StudentId} already enrolled to Course with Id:{entity.CourseId}");

            await base.AddAsync(new Enrollment
            {
                CourseId = entity.CourseId,
                StudentId = entity.StudentId,
            });
        }
    }
}

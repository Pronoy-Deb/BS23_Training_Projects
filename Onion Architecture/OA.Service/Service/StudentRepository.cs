using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OA.Data.Model;
using OA.Repo.Data;
using OA.Repo.Repository;

namespace OA.Service.Service
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context) : base(context) { }

        public async Task<Student?> GetStudentDetailsByIdAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Profile)
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public override async Task<Student?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Profile)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}

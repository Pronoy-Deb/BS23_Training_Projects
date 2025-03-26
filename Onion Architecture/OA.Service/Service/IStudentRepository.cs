using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OA.Data.Model;
using OA.Repo.Repository;

namespace OA.Service.Service
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task AddAsync(Student student);
        Task<Student?> GetStudentDetailsByIdAsync(int id);
    }
}

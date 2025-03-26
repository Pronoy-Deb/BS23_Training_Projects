using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OA.Data.Model;

namespace OA.Repo.Data.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name).IsRequired().HasMaxLength(100);

            // One-to-One with StudentProfile
            builder.HasOne(s => s.Profile)
                   .WithOne(p => p.Student)
                   .HasForeignKey<StudentProfile>(p => p.Id) // Shared PK
                   .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many with Enrollment
            builder.HasMany(s => s.Enrollments)
                   .WithOne(e => e.Student)
                   .HasForeignKey(e => e.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

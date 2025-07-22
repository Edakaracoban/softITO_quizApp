using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using QuizProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Data.Configurations
{
    public class TestTypeConfiguration : IEntityTypeConfiguration<TestType>
    {
        public void Configure(EntityTypeBuilder<TestType> builder)
        {
            builder.HasMany(tt => tt.Quizzes)
               .WithOne(q => q.TestType)
               .HasForeignKey(q => q.TestTypeId)
               .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

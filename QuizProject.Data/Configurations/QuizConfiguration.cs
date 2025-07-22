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
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasOne(q => q.TestType)
                .WithMany(tt => tt.Quizzes)
                .HasForeignKey(q => q.TestTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(q => q.Category)
                   .WithMany(c => c.Quizzes)
                   .HasForeignKey(q => q.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(q => q.Questions)
                   .WithOne(qst => qst.Quiz)
                   .HasForeignKey(qst => qst.QuizId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(q => q.UserQuizResults)
                   .WithOne(uqr => uqr.Quiz)
                   .HasForeignKey(uqr => uqr.QuizId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(q => q.QuizComments)
                   .WithOne(qc => qc.Quiz)
                   .HasForeignKey(qc => qc.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

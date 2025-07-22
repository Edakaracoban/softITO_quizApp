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
    public class UserQuizResultConfiguration : IEntityTypeConfiguration<UserQuizResult>
    {
        public void Configure(EntityTypeBuilder<UserQuizResult> builder)
        {
            builder.HasOne(uqr => uqr.User)
        .WithMany(u => u.UserQuizResults) // Modelinde bu ICollection varsa yaz, yoksa bırak boş.
        .HasForeignKey(uqr => uqr.UserId)
        .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(uqr => uqr.Quiz)
                   .WithMany(q => q.UserQuizResults)
                   .HasForeignKey(uqr => uqr.QuizId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(uqr => uqr.UserAnswers)
                   .WithOne(ua => ua.UserQuizResult)
                   .HasForeignKey(ua => ua.UserQuizResultId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(uqr => uqr.QuizComments)
                   .WithOne(qc => qc.UserQuizResult)
                   .HasForeignKey(qc => qc.UserQuizResultId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

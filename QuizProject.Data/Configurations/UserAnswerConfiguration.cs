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
    public class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
    {
        public void Configure(EntityTypeBuilder<UserAnswer> builder)
        {
            builder.HasOne(ua => ua.UserQuizResult)
        .WithMany(uqr => uqr.UserAnswers)
        .HasForeignKey(ua => ua.UserQuizResultId)
        .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ua => ua.Question)
                   .WithMany(q => q.UserAnswers)
                   .HasForeignKey(ua => ua.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

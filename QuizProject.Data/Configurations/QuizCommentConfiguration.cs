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
    public class QuizCommentConfiguration : IEntityTypeConfiguration<QuizComment>
    {
        public void Configure(EntityTypeBuilder<QuizComment> builder)
        {
            builder.HasOne(qc => qc.Quiz)
                   .WithMany(q => q.QuizComments)
                   .HasForeignKey(qc => qc.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(qc => qc.User)
                    .WithMany(u => u.QuizComments) // User tarafında ICollection<QuizComment> varsa bunu yazmak daha net olur
                    .HasForeignKey(qc => qc.UserId)
                    .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(qc => qc.UserQuizResult)
                   .WithMany(uqr => uqr.QuizComments)
                   .HasForeignKey(qc => qc.UserQuizResultId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using QuizProject.Data.Models;

namespace QuizProject.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.Property(q => q.Text)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasOne(q => q.Quiz)
                   .WithMany(qz => qz.Questions)
                   .HasForeignKey(q => q.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Diğer Question'a özel konfigürasyonlar buraya
        }
    }
}

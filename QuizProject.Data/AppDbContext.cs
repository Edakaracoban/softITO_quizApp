using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizProject.Data.Models;

namespace QuizProject.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<TestType> TestTypes { get; set; }
        public DbSet<UserQuizResult> UserQuizResults { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<QuizComment> QuizComments { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
        
    }
}


//**


//🔄 İlişkiler:
//Quiz → Question(1 - N)

//Quiz → Category (N-1)

//Quiz → TestType (N-1)

//UserQuizResult → User + Quiz (N-1)

//UserAnswer → Question + UserQuizResult (N-1)

//QuizComment → Quiz + User (nullable) (N-1)
//  protected override void OnModelCreating(ModelBuilder builder) //Fluent Api
//  {
//      base.OnModelCreating(builder);

//      builder.Entity<UserQuizResult>()//bir kullanıcının birden fazla sonucu olabilir.
//          .HasOne(uqr => uqr.User)
//          .WithMany()
//          .HasForeignKey(uqr => uqr.UserId)
//          .OnDelete(DeleteBehavior.SetNull);

//      builder.Entity<UserQuizResult>()//bir quizin birden çok sonucu olabilir.
//          .HasOne(uqr => uqr.Quiz)
//          .WithMany(q => q.UserQuizResults)
//          .HasForeignKey(uqr => uqr.QuizId)
//          .OnDelete(DeleteBehavior.SetNull);

//      builder.Entity<Question>()//bir quizde birden fazla soru olabilir.
//          .HasOne(q => q.Quiz)
//          .WithMany(quiz => quiz.Questions)
//          .HasForeignKey(q => q.QuizId)
//          .OnDelete(DeleteBehavior.SetNull);

//      builder.Entity<Quiz>()//her quiz bir test türüne ait olabilir.Bir testtype da birden fazla quiz olabilir.
//          .HasOne(q => q.TestType)
//          .WithMany(tt => tt.Quizzes)
//          .HasForeignKey(q => q.TestTypeId)
//          .OnDelete(DeleteBehavior.SetNull);

//      builder.Entity<Quiz>()//her quiz bir kategoriye ait olabilir.bir kategoride birden çok quiz olabilir.
//          .HasOne(q => q.Category)
//          .WithMany(c => c.Quizzes)
//          .HasForeignKey(q => q.CategoryId)
//          .OnDelete(DeleteBehavior.SetNull);

//      // ✅ DÜZENLENDİ: QuizComment - Quiz ilişkisi
//      builder.Entity<QuizComment>()//her yorum bir quiz ile ilişkilidir.bir quiz birden fazşa yoruma sahip olabilir.//quiz silindiğinde tüm ilgili yorumlar da silinir.
//          .HasOne(qc => qc.Quiz)
//          .WithMany(q => q.QuizComments)
//          .HasForeignKey(qc => qc.QuizId)
//          .OnDelete(DeleteBehavior.Cascade);

//      builder.Entity<QuizComment>()//Kullanıcıya ait yorumlar //Yorum yapan kullanıcı opsiyoneldir (anonim yorum olabilir).olabilir.
//          .HasOne(qc => qc.User)
//          .WithMany()
//          .HasForeignKey(qc => qc.UserId)
//          .OnDelete(DeleteBehavior.SetNull);

//      builder.Entity<QuizComment>()
//.HasOne(qc => qc.UserQuizResult)
//.WithMany(uqr => uqr.QuizComments)  // Burada koleksiyon ile bağlandı
//.HasForeignKey(qc => qc.UserQuizResultId)
//.OnDelete(DeleteBehavior.SetNull);  // veya istediğin davranış



//      builder.Entity<UserAnswer>() // her kullanıc cevabı bir kullanıcı kquiz sonucuna bağlıdır.bir quizresult birden çok sonuç içerebilir.
//  .HasOne(ua => ua.UserQuizResult)
//  .WithMany(uqr => uqr.UserAnswers)
//  .HasForeignKey(ua => ua.UserQuizResultId)
//  .OnDelete(DeleteBehavior.Cascade);

//      // UserAnswer - Question ilişkisi
//      builder.Entity<UserAnswer>()
//          .HasOne(ua => ua.Question)
//          .WithMany(q => q.UserAnswers)
//          .HasForeignKey(ua => ua.QuestionId)
//          .OnDelete(DeleteBehavior.Cascade);

//  }






//Doğru bir karar. Quiz silinince soru silinmiyor ama QuizId = null oluyor.
//Bu, “önceden hangi quiz'e aitti” sorusunu da cevapsız bırakmaz.
//Kullanıcı silinirse yorumu tutuyorsun ama anonim yapıyorsun. ✅ İyi UX.
//Quiz silinirse yorumlar da siliniyor → mantıklı çünkü bağlamı kalmıyor.
//QuizResult veya Question silinince cevapları da siliyorsun. ✅ Doğru ve tutarlı davranış.
//Kullanıcı silinse bile quiz geçmişi korunuyor. ✅ Raporlama için mükemmel tercih.
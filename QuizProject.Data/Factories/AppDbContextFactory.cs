using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace QuizProject.Data.Models
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            //optionsBuilder.UseSqlServer("Server=localhost;Database=QuizProject;User Id=SA;Password=reallyStrongPwd123;TrustServerCertificate=True;");

            optionsBuilder.UseSqlServer("Server=EDANIN-DESKTOPU\\SQLEXPRESS;Database=QuizProject;User Id=sa;Password=1;TrustServerCertificate=True;");


            return new AppDbContext(optionsBuilder.Options);
        }
    }
}


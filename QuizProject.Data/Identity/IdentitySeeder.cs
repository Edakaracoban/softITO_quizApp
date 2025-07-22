using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using QuizProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizProject.Data.Identity
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            string[] roles = new[] { Roles.Admin, Roles.Teacher, Roles.Student };

            // Roller oluşturuluyor
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Admin oluşturuluyor
            await CreateUserAsync(userManager, new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@mysite.com",
                FullName = "Eda Karaçoban Admin",
                EmailConfirmed = true,
                ProfileImageUrl = "/img/profile/admin.jpg",
                BirthDate = new DateTime(1997, 9, 16)
            }, "Admin123!", Roles.Admin);

            // Öğretmenler oluşturuluyor
            await CreateUserAsync(userManager, new ApplicationUser
            {
                UserName = "teacher1",
                Email = "teacher1@mysite.com",
                FullName = "John Wick Teacher",
                PhoneNumber= "05414003518",
                EmailConfirmed = true,
                ProfileImageUrl = "/img/profile/johnwick.jpg",
                BirthDate = new DateTime(1964, 9, 2)
            }, "Teacher123!", Roles.Teacher);

            await CreateUserAsync(userManager, new ApplicationUser
            {
                UserName = "teacher2",
                Email = "teacher2@mysite.com",
                FullName = "Jane Fonda Teacher",
                PhoneNumber = "05414003518",
                EmailConfirmed = true,
                ProfileImageUrl = "/img/profile/janefonda.jpg",
                BirthDate = new DateTime(1937, 12, 21)
            }, "Teacher123!", Roles.Teacher);

            // Öğrenciler oluşturuluyor ve ID'leri toplanıyor
            var students = new[]
            {
                new ApplicationUser
                {
                    UserName = "student1",
                    Email = "student1@mysite.com",
                    FullName = "Emma Watson Student",
                             PhoneNumber= "05414003518",
                    EmailConfirmed = true,
                    ProfileImageUrl = "/img/profile/emmawatson.jpg",
                    BirthDate = new DateTime(1990, 4, 15)
                },
                new ApplicationUser
                {
                    UserName = "student2",
                    Email = "student2@mysite.com",
                    FullName = "Daniel Radcliffe Student",
                             PhoneNumber= "05414003518",
                    EmailConfirmed = true,
                    ProfileImageUrl = "/img/profile/danielradcliffe.jpg",
                    BirthDate = new DateTime(1989, 7, 23)
                },

                new ApplicationUser
                {
                    UserName = "student3",
                    Email = "student3@mysite.com",
                    FullName = "Theo James Student",
                     PhoneNumber= "05414003518",
                    EmailConfirmed = true,
                    ProfileImageUrl = "/img/profile/theojames.jpeg",
                    BirthDate = new DateTime(1990, 1, 12)
                },
                  new ApplicationUser
                {
                    UserName = "student4",
                    Email = "student@mysite.com",
                     PhoneNumber= "05446368995",
                    FullName = "Agnes Student",
                    EmailConfirmed = true,
                    ProfileImageUrl = "/img/profile/agnestudent.jpeg",
                    BirthDate = new DateTime(2010,2,12) }
            };

            var studentIds = new Dictionary<string, string?>();

            foreach (var student in students)
            {
                var id = await CreateUserAsync(userManager, student, "Student123!", Roles.Student);
                studentIds.Add(student.UserName, id);
            }

            // UserQuizResults için örnek kayıtlar
            var userQuizResults = new List<UserQuizResult>();

            if (studentIds.TryGetValue("student1", out var student1Id) && student1Id != null)
            {
                userQuizResults.Add(new UserQuizResult
                {
                    UserId = student1Id,
                    QuizId = 1,
                    CorrectAnswers = 7,
                    TotalQuestions = 10,
                    TakenAt = DateTime.Now.AddDays(-10)
                });
            }

            if (studentIds.TryGetValue("student2", out var student2Id) && student2Id != null)
            {
                userQuizResults.Add(new UserQuizResult
                {
                    UserId = student2Id,
                    QuizId = 3,
                    CorrectAnswers = 9,
                    TotalQuestions = 10,
                    TakenAt = DateTime.Now.AddDays(-5)
                });
            }

            if (studentIds.TryGetValue("student3", out var student3Id) && student3Id != null)
            {
                userQuizResults.Add(new UserQuizResult
                {
                    UserId = student3Id,
                    QuizId = 5,
                    CorrectAnswers = 6,
                    TotalQuestions = 10,
                    TakenAt = DateTime.Now.AddDays(-7)
                });
            }

            if (userQuizResults.Count > 0)
            {
                dbContext.UserQuizResults.AddRange(userQuizResults);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task<string?> CreateUserAsync(UserManager<ApplicationUser> userManager, ApplicationUser user, string password, string role)
        {
            var existingUser = await userManager.FindByEmailAsync(user.Email!);
            if (existingUser != null)
            {
                // Kullanıcı varsa ama rolü yoksa role ata
                var roles = await userManager.GetRolesAsync(existingUser);
                if (!roles.Contains(role))
                {
                    await userManager.AddToRoleAsync(existingUser, role);
                }

                return existingUser.Id;
            }

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
                return user.Id;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    throw new Exception($"Identity seed error: {error.Description}");
                }
            }

            return null;
        }


    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizProject.Business.Interfaces;
using QuizProject.Business.Services;
using QuizProject.Data;
using QuizProject.Data.Identity;
using QuizProject.Data.Models;
using QuizProject.Data.Repositories.Abstract;
using QuizProject.Data.Repositories.Concrete;
using QuizProject.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// DbContext ve SQL Server Bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IUserQuizResultRepository, UserQuizResultRepository>();
builder.Services.AddScoped<ITestTypeRepository, TestTypeRepository>();
builder.Services.AddScoped<IUserAnswerRepository, UserAnswerRepository>();
builder.Services.AddScoped<IQuizCommentRepository, QuizCommentRepository>();

// Business Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ITestTypeService, TestTypeService>();
builder.Services.AddScoped<IUserQuizResultService, UserQuizResultService>();
builder.Services.AddScoped<IQuizCommentService, QuizCommentService>();
builder.Services.AddScoped<IUserAnswerService, UserAnswerService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Identity Konfigürasyonu
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Cookie Ayarları
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.SlidingExpiration = true;
});

// Session ekleme
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// MVC Ayarları
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7097/");
});
var app = builder.Build();

// Veritabanı seed işlemleri için scope açılıyor
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Roller ve kullanıcılar burada oluşturuluyor
    await IdentitySeeder.SeedRolesAndUsersAsync(services);
}

// Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();  // SESSION MIDDLEWARE EKLENDİ

app.UseAuthentication();
app.UseAuthorization();

// Area route (varsa)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.Run();

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Models.ViewModels
{
    public class UserEditVM
    {
        public string Id { get; set; } = null!; // Kullanıcı id
        public string FullName { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public string? ProfileImageUrl { get; set; }
    }

}
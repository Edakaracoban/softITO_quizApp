using Microsoft.AspNetCore.Mvc.Rendering;
using QuizProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Models.ViewModels
{
    public class CategoryVM
    {
        // Formdan dönen veya oluşturulan kategori
        public Category Category { get; set; } = new Category();

        // Kategori seçimi için dropdown listesi
        public IEnumerable<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();

        // Seçilen kategori ID'si (eğer sadece seçim yapılıyorsa)
        public int? SelectedCategoryId { get; set; }
    }
}
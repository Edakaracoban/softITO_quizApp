using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Models.ViewModels
{
    public class QuizSelectionVM
    {
        public int SelectedCategoryId { get; set; }
        public int SelectedTestTypeId { get; set; }
        public int SelectedQuizId { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TestTypeList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> QuizList { get; set; } = new List<SelectListItem>();
    }
}

//Bu durumda ViewModel'de üç dropdown listesi olmalı:

//CategoryList (Kategori seçimi için)

//TestTypeList (Kategori seçilince ona bağlı türler)

//QuizList (Tür seçilince ona bağlı quizler)
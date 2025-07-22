using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizProject.Data.Models;

namespace QuizProject.Models.ViewModels
{
    public class QuizVM
    {
        public Quiz Quiz { get; set; } = new Quiz();

        // Dropdown listeleri için
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TestTypes { get; set; } = new List<SelectListItem>();
    }

}
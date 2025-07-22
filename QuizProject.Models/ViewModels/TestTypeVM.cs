using Microsoft.AspNetCore.Mvc.Rendering;
using QuizProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Models.ViewModels
{
    public class TestTypeVM
    {
        public TestType TestType { get; set; } = new TestType();

        // İstersen bu test türü ile ilişkili Quiz listesini dropdown olarak verebilirsin
        public IEnumerable<SelectListItem> QuizList { get; set; } = new List<SelectListItem>();
    }
}
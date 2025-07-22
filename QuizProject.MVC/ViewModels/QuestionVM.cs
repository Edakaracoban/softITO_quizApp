using Microsoft.AspNetCore.Mvc.Rendering;
using QuizProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Models.ViewModels
{
    public class QuestionVM
    {
        public Question Question { get; set; } = new Question();

        public IEnumerable<SelectListItem> QuizList { get; set; } = new List<SelectListItem>();
    }
}
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizProject.Models.ViewModels
{
    public class TestTypeListVM
    {
        public IEnumerable<SelectListItem> TestTypes { get; set; } = new List<SelectListItem>();
        public int SelectedTestTypeId { get; set; }
    }
}
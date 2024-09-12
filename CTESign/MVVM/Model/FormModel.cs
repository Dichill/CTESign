using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTESign.MVVM.Model
{
    public class Answer
    {
        public string? QuestionId { get; set; }
        public string? Answer1 { get; set; }
    }

    public class FormModel
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime SubmitDate { get; set; } = DateTime.Now;
        public string? Answers { get; set; }
    }
}

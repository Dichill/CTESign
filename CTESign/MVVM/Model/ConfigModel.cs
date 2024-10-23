using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTESign.MVVM.Model
{
    public class ConfigModel
    {
        public string? Department { get; set; }
        public DateTime IssueDate { get; set; }
        public string? AFKLogoPath { get; set; }
        public int AFKLogoSize { get; set; }
        public string? AFKSignageText { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF
{
    internal class Model
    {
        public class SelectedPart
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Manufacturer { get; set; }
            public string Characteristics { get; set; }
            public double Price { get; set; }
            public string Image { get; set; }
            public int PartTypeId { get; set; }
        }

        public class CompatibilityResult
        {
            public bool IsCompatible { get; set; }
            public string Message { get; set; }
        }
    }
}

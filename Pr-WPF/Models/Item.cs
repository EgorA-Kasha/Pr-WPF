using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Models
{
    internal abstract class Item
    {
        public string Name { get; protected set; }
        public string ImageName { get; protected set; }
    }
}

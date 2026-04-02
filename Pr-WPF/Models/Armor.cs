using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pr_WPF.Models
{
    internal class Armor : Item
    {
        public int DefenseBonus { get; private set; }

        public Armor(int level)
        {
            Name = $"Броня {level}";
            DefenseBonus = level;
            ImageName = "armor.jpg";
        }
    }
}

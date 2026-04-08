using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Models
{
    internal class Skelet : Enemy
    {
        public Skelet() 
        {
            Name = "Скелет";
            MaxHealth = 40;
            CurrentHealth = 40;
            BaseDamage = 10;
            BaseDefense = 5;
            ImageName = "skelet.jpg";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Models
{
    internal class PestovCMM : Skelet
    {
        public PestovCMM()
        {
            Name = "PestovCMM";
            MaxHealth = (int)(40 * 1.3);
            CurrentHealth = MaxHealth;
            BaseDamage = (int)(10 * 1.8);
            BaseDefense = (int)(5 * 0.6);
            ImageName = "PestovCMM.jpg";
        }
    }
}

using Pr_WPF.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF
{
    internal class Global
    {
        public static ХОЗМАГEntities db = new ХОЗМАГEntities();
        public static int? LoggedInAs = null;
        public static Movie MovieSelected = null;
        public static MovieDetails.T ScreeningSelected = null;
        public static int? SeatSelected = null;
        public static int price = 667;
    }
}

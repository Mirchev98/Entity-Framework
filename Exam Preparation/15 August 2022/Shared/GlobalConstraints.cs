using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.Shared
{
    public class GlobalConstraints
    {
        //Truck
        public const string TruckPlateRegex = @"[A-Z]{2}[0-9]{4}[A-Z]{2}$";
        public const int TruckVinLen = 17;

        //Client
        public const int NameMin = 3;
        public const int NameMax = 40;
        public const int NationalityMin = 2;
        public const int NationalityMax = 40;

        //Despacher
        public const int NameMinDespach = 2;
        public const int NameMaxDespach = 40;
    }
}

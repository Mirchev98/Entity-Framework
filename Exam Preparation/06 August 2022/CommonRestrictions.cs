using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers
{
    public static class CommonRestrictions
    {
        //Footballer
        public const int footballerNameMinLen = 2;
        public const int footballerNameMaxLen = 40;

        //Team
        public const int teamNameMinLen = 3;
        public const int teamNameMaxLen = 40;
        public const string teamNameRegex = @"^[A-Za-z0-9\s\.\-]{3,}$";
        public const int nationalityMinLen = 2;
        public const int nationalityMaxLen = 40;
    }
}

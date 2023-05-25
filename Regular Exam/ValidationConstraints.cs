using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames
{
    public static class ValidationConstraints
    {
        //Boardgame
        public const int BoardGameNameMin = 10;
        public const int BoardGameNameMax = 20;

        //Seller
        public const int SellerNameMin = 5;
        public const int SellerNameMax = 20;
        public const int SellerAddressMin = 2;
        public const int SellerAddressMax = 30;
        public const string WebRegex = @"^www\.[A-z0-9\-]+\.com$";

        //Creator
        public const int CreatorFirstNameMin = 2;
        public const int CreatorFirstNameMax = 7;
        public const int CreatorLastNameMin = 2;
        public const int CreatorLastNameMax = 7;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theatre.Data.Common
{
    public class Validations
    {
        //Theatre
        public const int TheatreMaxLenName = 30;
        public const int TheatreMinLenName = 4;
        public const int TheatreMinDirector = 4;
        public const int TheatreMaxDirector = 30;

        //Play
        public const int PlayTitleMax = 50;
        public const int PlayTitleMin = 4;
        public const int PlayDescriptionMax = 700;
        public const int PlayScreenwriterMax = 30;
        public const int PlayScreenwriterMin = 4;

        //Cast
        public const int CastMaxName = 30;
        public const int CastMinName = 4;
        public const string CastNumberRegex = @"^\+44\-[\d]{2}\-[\d]{3}\-[\d]{4}";
    }
}

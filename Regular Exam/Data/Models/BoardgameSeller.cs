using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Data.Models
{
    public class BoardgameSeller
    {
        [Required]
        public int BoardgameId  { get; set; }

        public Boardgame Boardgame { get; set; } = null!;

        [Required]
        public int SellerId  { get; set; }

        public Seller Seller { get; set; } = null!;
    }
}

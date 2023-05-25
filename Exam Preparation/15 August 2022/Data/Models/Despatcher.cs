using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Trucks.Shared;

namespace Trucks.Data.Models
{
    public class Despatcher
    {
        public Despatcher()
        {
            Trucks = new List<Truck>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(GlobalConstraints.NameMaxDespach, MinimumLength = GlobalConstraints.NameMinDespach)]
        public string Name { get; set; } = null!;

        public string Position { get; set; }

        public ICollection<Truck> Trucks { get; set; }
    }
}

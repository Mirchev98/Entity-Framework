using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Shared;

namespace Trucks.Data.Models
{
    public class Client
    {
        public Client()
        {
            ClientsTrucks = new List<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(GlobalConstraints.NameMax, MinimumLength = GlobalConstraints.NameMin)]
        public string Name { get; set; } = null!;

        [StringLength(GlobalConstraints.NationalityMax, MinimumLength = GlobalConstraints.NationalityMin)]
        public string Nationality { get; set; } = null!;

        public string Type { get; set; } = null!;

        public ICollection<ClientTruck> ClientsTrucks { get; set; }
    }
}

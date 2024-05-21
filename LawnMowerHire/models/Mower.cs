using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawnMowerHire
{
    public class Mower
    {
        [DisplayName("Mower Id")]
        public int MowerId { get; set; }
        [DisplayName("Mower Type")]
        public string MowerType { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

        //relates mower to bookings in a one to many relationship
        public virtual List<Booking> Bookings { get; set; }

        public Mower()
        {
            Bookings = new List<Booking>();
        }

        public override string ToString()
        {
            return $"{Make} {Model} - {MowerType}";
        }
    }
}

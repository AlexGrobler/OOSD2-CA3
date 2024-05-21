using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawnMowerHire
{
    public class Booking
    {
        [DisplayName("Booking Id")]
        public int BookingId { get; set; }
        [DisplayName("Rent Date")]
        public DateTime RentDate { get; set; }
        [DisplayName("Return Date")]
        public DateTime ReturnDate { get; set; }

        //foreign key
        public int MowerId { get; set; }
        //allows reference to mower obj
        public virtual Mower Mower { get; set; }
    }
}

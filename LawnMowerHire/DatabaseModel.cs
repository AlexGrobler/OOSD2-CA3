using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawnMowerHire
{
    public class MowerHireData : DbContext
    {
        public DbSet<Mower> Mowers { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public MowerHireData() : base("MowerHire2024") { }
    }
}

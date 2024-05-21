using LawnMowerHire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MowerHireData db = new MowerHireData();

            using (db)
            {
                //bookings
                Booking booking1 = new Booking()
                {
                    RentDate = DateTime.Now,
                    ReturnDate = DateTime.Now
                };
                Booking booking2 = new Booking()
                {
                    RentDate = DateTime.Now,
                    ReturnDate = DateTime.Now
                };
                Booking booking3 = new Booking()
                {
                    RentDate = DateTime.Now,
                    ReturnDate = DateTime.Now
                };

                //mowers
                Mower sitOn = new Mower()
                {
                    MowerType = "Sit On",
                    Make = "Honda",
                    Model = "HF 2417 HB"
                };
                Mower strimmer = new Mower()
                {
                    MowerType = "Strimmer",
                    Make = "Husqvarna",
                    Model = "Weed Eater 320iL"
                };
                Mower pushMower = new Mower()
                {
                    MowerType = "Push Mower",
                    Make = "DeWalt",
                    Model = "DW33"
                };

                sitOn.Bookings.Add(booking1);   
                strimmer.Bookings.Add(booking2);
                strimmer.Bookings.Add(booking3);

                db.Mowers.Add(sitOn);   
                db.Mowers.Add(pushMower);
                db.Mowers.Add(strimmer);
                db.SaveChanges();
            }
        }
    }
}

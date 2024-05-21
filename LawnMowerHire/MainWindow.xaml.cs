using System.Windows;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Runtime.Remoting.Contexts;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Threading;

namespace LawnMowerHire
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MowerHireData db;
        private ObservableCollection<Mower> mowers;


        public MainWindow()
        {
            InitializeComponent();
            db = new MowerHireData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMowerListBox();
            LoadMowerTypes();
            LoadMowersTable();
            LoadBookingsTable();
            ResetMowerDetails();
        }

        private void ResetMowerDetails()
        {
            mowerIdTxt.Text = string.Empty;
            mowerMakeTxt.Text = string.Empty;
            mowerModelTxt.Text = string.Empty;
            mowerRentTxt.Text = string.Empty;
            mowerReturnTxt.Text = string.Empty;
        }

        private void LoadMowerListBox()
        {
            mowers = new ObservableCollection<Mower>(db.Mowers.ToList());
            mowerLstBx.ItemsSource = mowers;
        }

        private void LoadMowerTypes()
        {
            var mowerTypes = new List<string> {"All", "Strimmer", "Sit On", "Push Mower" };
            mowerTypeCmbBx.ItemsSource = mowerTypes;
            mowerTypeCmbBx.SelectedIndex = 0;
        }

        private void LoadMowersTable()
        {
            mowerTable.ItemsSource = db.Mowers.Select(m => new {
                m.MowerId,
                m.MowerType,
                m.Model,
                m.Make,
                Bookings = m.Bookings.Count
            }).ToList();
        }

        private void LoadBookingsTable()
        {
            bookingsTable.ItemsSource = db.Bookings.Select(b => new {
                b.BookingId,
                b.RentDate,
                b.ReturnDate,
                b.Mower.Make,
                b.Mower.Model
            }).ToList();
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            string selectedType = mowerTypeCmbBx.SelectedItem.ToString();
            DateTime? startDate = startDatePckr.SelectedDate;
            DateTime? endDate = endDatePckr.SelectedDate;

            if (selectedType == null || startDate == null || endDate == null)
            {
                MessageBox.Show("Please fill in all details");
                return;
            }

            if (startDate > endDate) 
            {
                MessageBox.Show("Return date must be after booking date");
                return;
            }

            List<Mower> mowers = new List<Mower>();

            if (selectedType == "All")
            {
                mowers = db.Mowers.ToList();
            }
            else 
            {
                mowers = db.Mowers.Where(m => m.MowerType == selectedType).ToList(); 
            }

            //create list of mowers where mower type == selected type, and none of its booking dates overlap wtih desired date
            List<Mower> availableMowers = mowers.Where(m => !m.Bookings.Any(b => b.RentDate <= endDate && b.ReturnDate >= startDate)).ToList();

            mowerLstBx.ItemsSource = availableMowers;
        }

        private void bookBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mowerLstBx.SelectedItem != null)
            {
                DateTime? startDate = startDatePckr.SelectedDate;
                DateTime? endDate = endDatePckr.SelectedDate;
                Mower mower = (Mower)mowerLstBx.SelectedItem;

                //need to also check if the selected date overlaps with existing date
                //also make sure start date isn't greater than end date
                if (startDate == null || endDate == null)
                {
                    MessageBox.Show("Please choose a date");
                    return;
                }

                if (startDate > endDate)
                {
                    MessageBox.Show("Return date must be after booking date");
                    return;
                }

                //get all bookins within the same range as the desired booking, check if they overlap
                List<Booking> existingBookings = mower.Bookings.Where(b => b.RentDate <= endDate && startDate <= b.ReturnDate).ToList();
                if (existingBookings.Count() > 0) 
                {
                    MessageBox.Show("Already have a booking or bookings overlapping with that date");
                    return;
                }

                Booking booking = new Booking()
                {
                    MowerId = mower.MowerId,
                    RentDate = startDate.Value,
                    ReturnDate = endDate.Value
                };

                db.Bookings.Add(booking);
                db.SaveChanges();
                LoadBookingsTable();

                MessageBox.Show($"Booking for {mower.Make} confirmed,\n Start Date {booking.RentDate.ToShortDateString()},\n End Date {booking.ReturnDate.ToShortDateString()}");

            }
            else
            {
                MessageBox.Show("Please select a valid mower to book");
            }
        }


        private void deleteBookingBtn_Click(object sender, RoutedEventArgs e)
        {
            if (bookingsTable.SelectedItem != null)
            {
                var selectedEntry = (dynamic)bookingsTable.SelectedItem;
                int bookingId = selectedEntry.BookingId;
                Booking booking = db.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
                if (booking != null)
                {
                    db.Bookings.Remove(booking);
                    db.SaveChanges();
                    LoadBookingsTable();
                }
                else 
                {
                    MessageBox.Show("Could not find booking with this Id");
                }
            }
        }

        private void mowerLstBx_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox.SelectedItem != null)
            {
                Mower mower = (Mower)listBox.SelectedItem;
                mowerIdTxt.Text = "Mower Id: " + mower.MowerId;
                mowerMakeTxt.Text = "Make: " + mower.Make;
                mowerModelTxt.Text = "Model: " + mower.Model;
                DateTime? rentDate = mower.Bookings?.FirstOrDefault()?.RentDate;
                DateTime? returnDate = mower.Bookings?.FirstOrDefault()?.ReturnDate;
                mowerRentTxt.Text = "Rental Date: " + (rentDate != null ? rentDate.Value.ToShortDateString() : "");
                mowerReturnTxt.Text = "Return Date: " + (returnDate != null ? returnDate.Value.ToShortDateString() : "");


                /*      if (mower.MowerType == "Strimmer")
                      {
                          mowerImg.Source = new BitmapImage(new Uri($"pack://application:,,,/OOSD2-CA3;component/images/strimmer.png"));
                      }*/
            }
            else
            {
                ResetMowerDetails();
            }
        }
    }
}

using LoggerProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restoranas;
using Restoranas.Interfaces;
using Restoranas.Models;
using Restoranas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoranas.Tests
{
    [TestClass()]
    public class ProjectRestaurantTests
    {
        [TestMethod()]
        public void OrderMealTest() //Patikriname ar prisideda užsakomas patiekalas į sąrašą
        {
            ILoggerService logger = new NoLogger();
            IEmailService emailService = new NoEmail();
            OrderService orderService = new OrderService(logger, emailService);

            Table tableMealExpected = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            Food food = new Food(1, "Zuvis", 10, "", "", true);
            tableMealExpected.Order.Meals.Add(food);

            Table table = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            table = orderService.OrderMeal(table, food);

            CollectionAssert.AreEqual(tableMealExpected.Order.Meals, table.Order.Meals);
        }
        [TestMethod()]
        public void OrderMealTableReservedTest() //Patikriname ar užsirezervuoja staliukas pridedant užsakymą staliukui
        {
            ILoggerService logger = new NoLogger();
            IEmailService emailService = new NoEmail();
            OrderService orderService = new OrderService(logger, emailService);

            Food food = new Food(1, "Zuvis", 10, "", "", true);
            Table table = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            table = orderService.OrderMeal(table, food);

            Assert.AreEqual(true, table.IsReserved);
        }
        [TestMethod()]
        public void OrderMealNullTest() //Patikriname perduodami meal null value
        {
            ILoggerService logger = new NoLogger();
            IEmailService emailService = new NoEmail();
            OrderService orderService = new OrderService(logger, emailService);

            Table tableMealExpected = new Table(1, 6, false, new List<Reservation>(), new Order(), null);

            Table table = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            table = orderService.OrderMeal(table, null);

            CollectionAssert.AreEqual(tableMealExpected.Order.Meals, table.Order.Meals);
        }
        [TestMethod()]
        public void DeleteMealFromOrderTest() //Patikriname ar pasišalina patiekalas iš sąrašo
        {
            ILoggerService logger = new NoLogger();
            IEmailService emailService = new NoEmail();
            OrderService orderService = new OrderService(logger, emailService);

            Table tableMealExpected = new Table(1, 6, false, new List<Reservation>(), new Order(), null);

            Table table = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            Food food = new Food(1, "Zuvis", 10, "", "", true);
            table.Order.Meals.Add(food);
            orderService.DeleteMealFromOrder(table, 0);

            CollectionAssert.AreEqual(tableMealExpected.Order.Meals, table.Order.Meals);


        }
        [TestMethod()]
        public void DeleteMealFromOrderTwoOrdersOneRemovedOrdersTest() //Patikriname ar pasišalina patiekalas iš sąrašo kai yra keli patiekalai sąraše
        {
            ILoggerService logger = new NoLogger();
            IEmailService emailService = new NoEmail();
            OrderService orderService = new OrderService(logger, emailService);

            Food food = new Food(1, "Zuvis", 10, "", "", true);
            Food food2 = new Food(1, "Kepsnys", 15, "", "", true);

            Table tableMealExpected = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            tableMealExpected.Order.Meals.Add(food);

            Table table = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            table.Order.Meals.Add(food);
            table.Order.Meals.Add(food2);
            orderService.DeleteMealFromOrder(table, 1);

            CollectionAssert.AreEqual(tableMealExpected.Order.Meals, table.Order.Meals);
        }
        [TestMethod()]
        public void DeleteMealFromOrderNonExistingIndexTest() //Patikriname patiekalo pašalinimą iš sąrašo kai nėra tokio patiekalo numerio sąraše (index)
        {
            ILoggerService logger = new NoLogger();
            IEmailService emailService = new NoEmail();
            OrderService orderService = new OrderService(logger, emailService);

            Food food = new Food(1, "Zuvis", 10, "", "", true);
            Food food2 = new Food(1, "Kepsnys", 15, "", "", true);

            Table tableMealExpected = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            tableMealExpected.Order.Meals.Add(food);
            tableMealExpected.Order.Meals.Add(food2);

            Table table = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            table.Order.Meals.Add(food);
            table.Order.Meals.Add(food2);
            orderService.DeleteMealFromOrder(table, 999);

            CollectionAssert.AreEqual(tableMealExpected.Order.Meals, table.Order.Meals);
        }
        [TestMethod()]
        public void PayOrderClearMealOrdersAndReservationFalseTest() //Patikriname ar nusiima rezervacija ir išsivalo užsakymas apmokant
        {
            ILoggerService logger = new NoLogger();
            IEmailService emailService = new NoEmail();
            OrderService orderService = new OrderService(logger, emailService);

            Food food = new Food(1, "Zuvis", 10, "", "", true);
            Food food2 = new Food(1, "Kepsnys", 15, "", "", true);

            Table tableMealExpected = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            tableMealExpected.IsReserved = false;
         
            

            Table table = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            table.Order.Meals.Add(food);
            table.Order.Meals.Add(food2);
            table.IsReserved = true;

            orderService.PayOrder(table, "", null);

            CollectionAssert.AreEqual(tableMealExpected.Order.Meals, table.Order.Meals);
            Assert.IsFalse(table.IsReserved);
        }
        [TestMethod()]
        public void ReserveTableTest() //Patikrinimas ar prisideda rezervacija į staliuko rezervacijų sąrašą
        {
            ILoggerService logger = new NoLogger();
            IEmailService emailService = new NoEmail();
            ReservationService reservationService = new ReservationService(logger, emailService);
            DateTime date = DateTime.Now;
            Table tableExpected = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            tableExpected.Reservations.Add(new Reservation("Petras", date, 6, "toma.bagu@gmail.com"));

            Table table = new Table(1, 6, false, new List<Reservation>(), new Order(), null);

            reservationService.ReserveTable(table, date, "Petras", 6, "toma.bagu@gmail.com");

            CollectionAssert.AreEqual(tableExpected.Reservations, table.Reservations);
        }

        [TestMethod()]
        public void ReserveTableSecondReservationLessThanTwoHoursApartTest() //Patikrinimas ar neprisideda rezervacija jeigu laikas yra mažesnis nei dvi valandos nuo pirmosios
        {
            ILoggerService logger = new NoLogger();
            IEmailService emailService = new NoEmail();
            ReservationService reservationService = new ReservationService(logger, emailService);
            DateTime date = DateTime.Now;
            Table tableExpected = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            tableExpected.Reservations.Add(new Reservation("Petras", date, 6, "toma.bagu@gmail.com"));

            Table table = new Table(1, 6, false, new List<Reservation>(), new Order(), null);

            reservationService.ReserveTable(table, date, "Petras", 6, "toma.bagu@gmail.com");
            reservationService.ReserveTable(table, date, "Petras", 6, "toma.bagu@gmail.com");

            CollectionAssert.AreEqual(tableExpected.Reservations, table.Reservations);
        }

        [TestMethod()]
        public void ReserveTableSecondReservationMoreThanTwoHoursApartTest() //Patikrinimas ar prisideda antra rezervacija dvi valandos po pirmos
        {
            ILoggerService logger = new NoLogger();
            IEmailService emailService = new NoEmail();
            ReservationService reservationService = new ReservationService(logger, emailService);
            DateTime date = DateTime.Now;
            DateTime dateTwoHoursAfter = DateTime.Now.AddHours(2);
            Table tableExpected = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            tableExpected.Reservations.Add(new Reservation("Petras", date, 6, "toma.bagu@gmail.com"));
            tableExpected.Reservations.Add(new Reservation("Petras", dateTwoHoursAfter, 6, "toma.bagu@gmail.com"));

            Table table = new Table(1, 6, false, new List<Reservation>(), new Order(), null);

            reservationService.ReserveTable(table, date, "Petras", 6, "toma.bagu@gmail.com");
            reservationService.ReserveTable(table, dateTwoHoursAfter, "Petras", 6, "toma.bagu@gmail.com");

            CollectionAssert.AreEqual(tableExpected.Reservations, table.Reservations);
        }

        [TestMethod()]
        public void ReserveTableNullTableTest() //Patikrinimas kai nurodomas staliukas null
        {
            ILoggerService logger = new NoLogger();
            IEmailService emailService = new NoEmail();
            ReservationService reservationService = new ReservationService(logger, emailService);
            DateTime date = DateTime.Now;

            Table table = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            table = reservationService.ReserveTable(null, date, "Petras", 6, "toma.bagu@gmail.com");

            Assert.AreEqual(null, table);
        }

        [TestMethod()]
        public void CancelReservationTest() //Patikrinimas kai yra atšaukiamas staliuko rezervacija ar pasišąlina iš sąrašo rezervacija
        {
            ILoggerService logger = new NoLogger();
            IEmailService emailService = new NoEmail();
            ReservationService reservationService = new ReservationService(logger, emailService);
            DateTime date = DateTime.Now;
            Table tableExpected = new Table(1, 6, false, new List<Reservation>(), new Order(), null);

            Reservation reservation = new Reservation("Petras", date, 6, "toma.bagu@gmail.com");
            Table table = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            table.Reservations.Add(reservation);

            reservationService.CancelReservation(table, 0);

            CollectionAssert.AreEqual(tableExpected.Reservations, table.Reservations);
        }

        [TestMethod()]
        public void CancelReservationMultipleReservationsCancelSingleTest() //Patikrinimas kai yra atšaukiamas staliuko rezervacija ar pasišąlina iš sąrašo rezervacija, kai yra kelios rezervacijos
        {
            ILoggerService logger = new NoLogger();
            IEmailService emailService = new NoEmail();
            ReservationService reservationService = new ReservationService(logger, emailService);
            DateTime date = DateTime.Now;

            Reservation reservation1 = new Reservation("Petras", date, 6, "toma.bagu@gmail.com");
            Reservation reservation2 = new Reservation("Petras2", date, 6, "toma.bagu@gmail.com");
            Table tableExpected = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            tableExpected.Reservations.Add(reservation2);

            Table table = new Table(1, 6, false, new List<Reservation>(), new Order(), null);
            table.Reservations.Add(reservation1);
            table.Reservations.Add(reservation2);

            reservationService.CancelReservation(table, 0);

            CollectionAssert.AreEqual(tableExpected.Reservations, table.Reservations);
        }
    }
}
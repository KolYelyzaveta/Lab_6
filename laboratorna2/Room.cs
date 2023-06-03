using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboratorna2
{
    public class Room
    {
        public int Number { get; set; }
        public int Places { get; set; }
        public int TakenPlaces { get; set; }
        public string Date { get; set; }
        public double Price { get; set; }
        public Status CurrentStatus { get; set; }

        public enum Status
        {
            FREE,
            LOCKED
        }

        public Room(int number, int places, int takenPlaces, string date, double price, Status status)
        {
            this.Number = number;
            this.Places = places;
            this.TakenPlaces = takenPlaces;
            this.Date = date;
            this.Price = price;
            this.CurrentStatus = status;
        }

        public Room() { }
    }
}

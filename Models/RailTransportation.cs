using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightMate.Models
{
    public class RailTransportation
    {
        public int id;
        public Carriage carriage;
        public Order order;
        public Guid token;
        public string status;

        public RailTransportation()
        {
            this.id = 0;
            this.carriage = new Carriage();
            this.order = new Order();
            this.token = new Guid();
            this.status = "";
        }

        public RailTransportation(int id, Carriage carriage, Order order, Guid token, string status)
        {
            this.id = id;
            this.carriage = carriage;
            this.order = order;
            this.token = token;
            this.status = status;
        }

        public int Id { get { return this.id; } set { this.id = value; } }
        public Carriage Carriage { get {  return this.carriage; } set { this.carriage = value; } }
        public Order Order { get { return this.order; } set { this.order = value; } }
        public Guid Token { get { return this.token; } set { this.token = value; } }
        public string Status {  get { return this.status; } set { this.status = value; } }
    }
}

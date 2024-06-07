using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightMate.Models
{
    public class Transportation
    {
        private int id;
        private Order order;
        private string driverName;
        private string status;
        private string warehouseLocation;
        private DateTime? getingCargoTime;

        public Transportation()
        {
            this.id = 0;
            this.order = new Order();
            this.driverName = "";
            this.status = "";
            this.warehouseLocation = "";
            this.getingCargoTime = DateTime.Now;
        }

        public Transportation(int id, Order order, string driverName, string status, string warehouse, DateTime? getingCargoTime)
        {
            this.id = id;  
            this.order = order;
            this.driverName = driverName;
            this.status = status;
            this.warehouseLocation = warehouse;
            this.getingCargoTime = getingCargoTime;
        }

        public int Id {  get { return this.id; } set { this.id = value; } }
        public Order Order { get { return this.order; } set { this.order = value; } }
        public string DriverName { get { return this.driverName; } set { this.driverName = value;} }
        public string Status { get { return this.status; } set { this.status = value; }  }
        public string WarehouseLocation { get { return this.warehouseLocation; } set { this.warehouseLocation = value; } }
        public DateTime? GetingCargoTime { get { return this.getingCargoTime; } set { this.getingCargoTime = value; } }
    }
}

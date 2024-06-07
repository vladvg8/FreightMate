using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightMate.Models
{
    public class Carriage
    {
        public int id;
        public CarriageType carrigeType;
        public Guid? railTransportationToken;
        public string status;

        public Carriage()
        {
            this.id = 0;
            this.carrigeType = new CarriageType();
            this.railTransportationToken = null ;
            this.status = "";
        }
        public Carriage(int id, CarriageType carrigeType, Guid railTransportationToken, string status)
        {
            this.id = id;
            this.carrigeType = carrigeType;
            this.railTransportationToken = railTransportationToken;
            this.status = status;
        }
        public int Id {  get { return this.id; } set { this.id = value; } }
        public CarriageType CarriageType {  get { return this.carrigeType;} set { this.carrigeType = value; } }
        public string Status { get { return this.status; } set { this.status = value; } }
        public Guid? RailTransportationToken {  get { return this.railTransportationToken;} set { this.railTransportationToken = value; } }
        public override string ToString()
        {
            return $"{id}, тип: {CarriageType.Name}";
        }
    }
}

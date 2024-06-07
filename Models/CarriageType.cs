using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightMate.Models
{
    public class CarriageType
    {
        public int id;
        public string name;
        public float maxWeight;
        public float maxVolume;
        public string description;
        public string photoUrl;

        public CarriageType()
        {
            this.id = 0;
            this.name = "";
            this.maxWeight = 0;
            this.maxVolume = 0;
            this.description = "";
            this.photoUrl = "";
        }
        public CarriageType(int id, string name, float maxWeight, float maxVolume, string description, string photoUrl)
        {
            this.id = id;
            this.name = name;
            this.maxWeight = maxWeight;
            this.maxVolume = maxVolume;
            this.description = description;
            this.photoUrl = photoUrl;
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public float MaxWeight { get { return this.maxWeight; } set { this.maxWeight = value; } }
        public float MaxVolume { get { return this.maxVolume; } set { this.maxVolume = value; } }
        public string Description { get { return this.description; } set { this.description = value; } }
        public string PhotoUrl { get { return this.photoUrl; } set { this.photoUrl = value; } }
    }
}

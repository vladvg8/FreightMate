using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FreightMate.Models
{
    public class Order
    {
        public int id;
        public Request request;
        public float price;
        public string status;
        public string? deniedMessage;
        public DateTime? expectedDeliveryTime;
        public Order()
        {
            id = 0;
            request = new Request();
            price = 0;
            status = "";
            deniedMessage = "";
            expectedDeliveryTime = DateTime.Now;
        }

        public Order(int id, Request request, float price, string status, string? deniedMessage, DateTime? expectedDeliveryTime)
        {
            this.id = id;
            this.request = request;
            this.price = price;
            this.status = status;
            this.deniedMessage = deniedMessage;
            this.expectedDeliveryTime = expectedDeliveryTime;
        }

        public int Id {  get { return id; } set { id = value; } }
        public Request Request { get { return request; } set { request = value; } }
        public string Price 
        { 
            get 
            { 
                return price.ToString(); 
            } 
            set 
            { 
                if (float.TryParse(value, out float newValue))
                {
                    if (newValue > 0)
                    {
                        price = newValue;
                    }
                    else
                    {
                        MessageBox.Show("Значение поля 'Цена' должна быть больше нуля");
                    }
                } else
                {
                    MessageBox.Show("Неверное значение");
                }
            } 
        }
        public string Status { get { return status; } set {  status = value; } }
        public string? DeniedMessage { get { return deniedMessage; } set { deniedMessage = value;} }
        public DateTime? ExpectedDeliveryTime { get { return expectedDeliveryTime; } set {  expectedDeliveryTime = value; } }

        public override string ToString()
        {
            return this.Id.ToString();
        }
    }
}

using Azure.Core;
using FreightMate.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FreightMate.ViewModels.User
{
    public class OrdersCommand : INotifyPropertyChanged
    {
        private List<Order> orders = DatabaseConnector.GetOrders();
        public List<Order> Orders
        {
            get
            {
                return orders;
            }
            set
            {
                orders = value;
                OnPropertyChanged("Orders");
            }
        }

        DefaultCommand? refresh;
        public DefaultCommand Refresh
        {
            get
            {
                return refresh ?? (refresh = new DefaultCommand((obj) =>
                {
                    Orders = DatabaseConnector.GetOrders();
                }));
            }
        }

        DefaultCommand? deleteOrder;
        public DefaultCommand DeleteOrder
        {
            get
            {
                return deleteOrder ?? (deleteOrder = new DefaultCommand((obj) =>
                {
                    if (obj is int orderId)
                    {
                        DatabaseConnector.DeleteOrder(orderId);
                        Orders = DatabaseConnector.GetOrders();
                    }
                }));
            }
        }

        DefaultCommand? chooseDeliveryTime;
        public DefaultCommand ChooseDeliveryTime
        {
            get
            {
                return chooseDeliveryTime ?? (chooseDeliveryTime = new DefaultCommand((obj) =>
                {
                    if (obj is int orderId)
                    {
                        Regex regex = new Regex(@"^(0[1-9]|[12][0-9]|3[01])\.(0[1-9]|1[012])\.\d{4} (0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
                        string? deliveryTime = Interaction.InputBox("Ожидамое время приема груза (дд.мм.гггг чч:мм)", "FreightMate - ожидаемое время приема груза");
                        if (deliveryTime != "" && regex.IsMatch(deliveryTime))
                        {
                            DateTime time = DateTime.ParseExact(deliveryTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                            if (time > DateTime.Now)
                            {
                                DatabaseConnector.SetDeliveryTimeOrder(orderId, time);
                                Orders = DatabaseConnector.GetOrders();
                            } else
                            {
                                MessageBox.Show("Неверная дата", "FreightMate - ожидаемое время доставки");
                            }
                        } else
                        {
                            MessageBox.Show("Неверная дата", "FreightMate - ожидаемое время доставки");
                        }
                    }
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}

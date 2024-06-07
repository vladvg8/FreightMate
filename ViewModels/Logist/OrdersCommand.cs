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

namespace FreightMate.ViewModels.Logist
{
    public class OrdersCommand : INotifyPropertyChanged
    {
        private List<Order> orders = DatabaseConnector.GetOrdersAdmin();
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
                    Orders = DatabaseConnector.GetOrdersAdmin().
                          Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                          Where(x => searchingRequestId == null || x.Request.Id == searchingRequestId).Select(x => x).
                          Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).ToList();
                }));
            }
        }

        private int? searchingId = null;
        public string SearchingId
        {
            get
            {
                return searchingId == null ? "" : searchingId.ToString();
            }
            set
            {
                if (value != "")
                {
                    if (int.TryParse(value, out int newValue))
                    {
                        if (newValue > 0)
                        {
                            this.searchingId = newValue;
                        }
                        else
                        {
                            this.searchingId = null;
                        }

                    }
                }
                else
                {
                    this.searchingId = null;
                }
                OnPropertyChanged("SearchingId");
            }
        }

        private int? searchingRequestId = null;
        public string SearchingRequestId
        {
            get
            {
                return searchingRequestId == null ? "" : searchingRequestId.ToString();
            }
            set
            {
                if (value != "")
                {
                    if (int.TryParse(value, out int newValue))
                    {
                        if (newValue > 0)
                        {
                            this.searchingRequestId = newValue;
                        }
                        else
                        {
                            this.searchingRequestId = null;
                        }
                    }
                }
                else
                {
                    this.searchingRequestId = null;
                }
                OnPropertyChanged("SearchingRequestId");
            }
        }

        private string? searchingStatus = null;
        public string? SearchingStatus
        {
            get
            {
                return searchingStatus;
            }
            set
            {
                searchingStatus = value;
                OnPropertyChanged("SearchingStatus");
            }
        }

        DefaultCommand? applyFilter;
        public DefaultCommand ApplyFilter
        {
            get
            {
                return applyFilter ?? (applyFilter = new DefaultCommand((obj) =>
                {
                    Orders = DatabaseConnector.GetOrdersAdmin().
                    Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                    Where(x => searchingRequestId == null || x.Request.Id == searchingRequestId).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).ToList();

                }));
            }
        }

        DefaultCommand? clearFilter;
        public DefaultCommand ClearFilter
        {
            get
            {
                return clearFilter ?? (clearFilter = new DefaultCommand((obj) =>
                {
                    searchingId = null;
                    searchingStatus = null;
                    searchingRequestId = null;
                    Orders = DatabaseConnector.GetOrdersAdmin();
                    OnPropertyChanged("SearchingId");
                    OnPropertyChanged("SearchingRequestId");
                    OnPropertyChanged("SearchingStatus");
                }));
            }
        }

        DefaultCommand? approveOrder;
        public DefaultCommand ApproveOrder
        {
            get
            {
                return approveOrder ?? (approveOrder = new DefaultCommand((obj) =>
                {
                    if (obj is int orderId)
                    {
                        Regex regex = new Regex(@"^(0[1-9]|[12][0-9]|3[01])\.(0[1-9]|1[012])\.\d{4} (0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
                        string? deliveryTime = Interaction.InputBox("Ожидамое время доставки заказа (дд.мм.гггг чч:мм)", "FreightMate - ожидаемое время доставки");
                        if (deliveryTime != "" && regex.IsMatch(deliveryTime))
                        {
                            DateTime time = DateTime.ParseExact(deliveryTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                            if (time > DateTime.Now)
                            {
                                DatabaseConnector.ApproveOrder(orderId, time);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Неверная дата", "FreightMate - ожидаемое время доставки");
                        }
                        Orders = DatabaseConnector.GetOrdersAdmin().
                    Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                    Where(x => searchingRequestId == null || x.Request.Id == searchingRequestId).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).ToList();
                    }
                }));
            }
        }

        DefaultCommand? denieOrder;
        public DefaultCommand DenieOrder
        {
            get
            {
                return denieOrder ?? (denieOrder = new DefaultCommand((obj) =>
                {
                    if (obj is int orderId)
                    {
                        string? denieMessage = Interaction.InputBox("Причина удаления", "FreightMate - причина удаления заказа");
                        denieMessage = denieMessage.Trim();
                        if (denieMessage != "")
                        {
                            DatabaseConnector.DenieOrder(orderId, denieMessage);
                            Orders = DatabaseConnector.GetOrdersAdmin().
                    Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                    Where(x => searchingRequestId == null || x.Request.Id == searchingRequestId).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).ToList();
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

﻿using Azure.Core;
using FreightMate.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FreightMate.ViewModels.Admin
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
                } else
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
                } else
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
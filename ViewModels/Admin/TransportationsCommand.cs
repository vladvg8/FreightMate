using FreightMate.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Controls;

namespace FreightMate.ViewModels.Admin
{
    public class TransportationsCommand : INotifyPropertyChanged
    {
        private List<FreightMate.Models.Transportation> transportations = DatabaseConnector.GetTransportationsAdmin();
        public List<Models.Transportation> Transportations
        {
            get
            {
                return transportations;
            }
            set
            {
                transportations = value;
                OnPropertyChanged("Transportations");
            }
        }

        DefaultCommand? refresh;
        public DefaultCommand Refresh
        {
            get
            {
                return refresh ?? (refresh = new DefaultCommand((obj) =>
                {
                    Transportations = DatabaseConnector.GetTransportationsAdmin().
                    Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                    Where(x => searchingOrderId == null || x.Order.Id == searchingOrderId).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingWarehouse) || x.WarehouseLocation == searchingWarehouse).ToList();
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

        private int? searchingOrderId = null;
        public string SearchingOrderId
        {
            get
            {
                return searchingOrderId == null ? "" : searchingOrderId.ToString();
            }
            set
            {
                if (value != "")
                {
                    if (int.TryParse(value, out int newValue))
                    {
                        if (newValue > 0)
                        {
                            this.searchingOrderId = newValue;
                        }
                        else
                        {
                            this.searchingOrderId = null;
                        }
                        
                    }
                } else
                {
                    this.searchingOrderId = null;

                }
                OnPropertyChanged("SearchingOrderId");
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

        private string? searchingWarehouse = null;
        public string? SearchingWarehouse
        {
            get
            {
                return searchingWarehouse;
            }
            set
            {
                searchingWarehouse = value;
                OnPropertyChanged("SearchingWarehouse");
            }
        }

        DefaultCommand? applyFilter;
        public DefaultCommand ApplyFilter
        {
            get
            {
                return applyFilter ?? (applyFilter = new DefaultCommand((obj) =>
                {
                    Transportations = DatabaseConnector.GetTransportationsAdmin().
                    Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                    Where(x => searchingOrderId == null || x.Order.Id == searchingOrderId).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingWarehouse) || x.WarehouseLocation == searchingWarehouse).ToList();

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
                    searchingOrderId = null;
                    searchingWarehouse = null;
                    Transportations = DatabaseConnector.GetTransportationsAdmin();
                    OnPropertyChanged("SearchingWarehouse");
                    OnPropertyChanged("SearchingId");
                    OnPropertyChanged("searchingOrderId");
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

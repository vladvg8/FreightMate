using FreightMate.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FreightMate.ViewModels.Logist
{
    public class TransportationsCommand : INotifyPropertyChanged
    {
        List<Transportation> transportations = DatabaseConnector.GetTransportationsAdmin();
        public List<Transportation> Transportations
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

        public List<Order> Orders
        {
            get
            {
                return DatabaseConnector.GetOrdersAdmin().Where(x => x.Status == "Активен").ToList();
            }
        }

        private int currentPage;
        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        DefaultCommand? openNewTransportation;
        public DefaultCommand OpenNewTransportation
        {
            get
            {
                return openNewTransportation ?? (openNewTransportation = new DefaultCommand((obj) =>
                {
                    CurrentPage = 1;
                }));
            }
        }

        DefaultCommand? openTransportationList;
        public DefaultCommand OpenTransportationList
        {
            get
            {
                return openTransportationList ?? (openTransportationList = new DefaultCommand((obj) =>
                {
                    CurrentPage = 0;
                }));
            }
        }

        string errorMessage;
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        DefaultCommand? addNewTransportation;
        public DefaultCommand AddNewTransportation
        {
            get
            {
                return addNewTransportation ?? (addNewTransportation = new DefaultCommand((obj) =>
                {
                    if (newTransportationOrderId != 0 && NewTransportation.DriverName != "" && NewTransportation.WarehouseLocation != "")
                    {
                        if (DatabaseConnector.AddNewTransportation(NewTransportation.DriverName, NewTransportation.WarehouseLocation, newTransportationOrderId))
                        {
                            ErrorMessage = "";
                            NewTransportation = new Transportation();
                            MessageBox.Show("Перевозка успешно добавлена");
                            CurrentPage = 0;
                            Transportations = DatabaseConnector.GetTransportationsAdmin().
Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
Where(x => searchingOrderId == null || x.Order.Id == searchingOrderId).Select(x => x).
Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).
Where(x => string.IsNullOrEmpty(searchingWarehouse) || x.WarehouseLocation == searchingWarehouse).ToList();
                        } else
                        {
                            ErrorMessage = "Ошибка добавления новой транспортировки\nПопробуйте обновить список заказов";
                        }
                    } else
                    {
                        ErrorMessage = "Заполните все поля";
                    }
                }));
            }
        }

        DefaultCommand? closeTransportation;
        public DefaultCommand CloseTransportation
        {
            get
            {
                return closeTransportation ?? (closeTransportation = new DefaultCommand((obj) =>
                {
                    if (obj is int transportationId)
                    {
                        if(DatabaseConnector.CloseTransportation(transportationId))
                        {
                            Transportations = DatabaseConnector.GetTransportationsAdmin().
Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
Where(x => searchingOrderId == null || x.Order.Id == searchingOrderId).Select(x => x).
Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).
Where(x => string.IsNullOrEmpty(searchingWarehouse) || x.WarehouseLocation == searchingWarehouse).ToList();
                        } else
                        {
                            MessageBox.Show("Невозможно закрыть перевозку");
                        }
                    }
                }));
            }
        }

        int newTransportationOrderId;
        public string NewTransportationOrderId
        {
            get
            {
                return this.newTransportationOrderId.ToString();
            }
            set
            {
                newTransportationOrderId = int.Parse(value);
                OnPropertyChanged("NewTransportationOrderId");
            }
        }

        Transportation newTransportation = new Transportation();
        public Transportation NewTransportation
        {
            get
            {
                return newTransportation;
            }
            set
            {
                newTransportation = value;
                OnPropertyChanged("NewTransportation");
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
                }
                else
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
                }
                else
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

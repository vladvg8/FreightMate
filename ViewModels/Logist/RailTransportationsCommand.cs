using FreightMate.Models;
using Microsoft.IdentityModel.Tokens;
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
    public class RailTransportationsCommand : INotifyPropertyChanged
    {
        List<RailTransportation> railsTransportations = DatabaseConnector.GetRailTransportations();
        public List<RailTransportation> RailTransportations
        {
            get
            {
                return railsTransportations;
            }
            set
            {
                railsTransportations = value;
                OnPropertyChanged("RailTransportations");
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

        DefaultCommand? openNewRailTransportation;
        public DefaultCommand OpenNewRailTransportation
        {
            get
            {
                return openNewRailTransportation ?? (openNewRailTransportation = new DefaultCommand((obj) =>
                {
                    ErrorMessage = "";
                    CurrentPage = 1;
                }));
            }
        }

        DefaultCommand? openRailTransportationList;
        public DefaultCommand OpenRailTransportationList
        {
            get
            {
                return openRailTransportationList ?? (openRailTransportationList = new DefaultCommand((obj) =>
                {
                    ErrorMessage = "";
                    CurrentPage = 0;
                }));
            }
        }

        public List<Carriage> Carriages
        {
            get
            {
                return DatabaseConnector.GetCarriages().Where(x => x.Status == "Свободен").ToList();
            }
        }

        public List<Order> Orders
        {
            get
            {
                return DatabaseConnector.GetOrdersAdmin().Where(x => x.Status == "На складе").ToList();
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

        private int? searchingCarriageId = null;
        public string SearchingCarriageId
        {
            get
            {
                return searchingCarriageId == null ? "" : searchingCarriageId.ToString();
            }
            set
            {
                if (value != "")
                {
                    if (int.TryParse(value, out int newValue))
                    {
                        if (newValue > 0)
                        {
                            this.searchingCarriageId = newValue;
                        }
                        else
                        {
                            this.searchingCarriageId = null;
                        }
                    }
                }
                else
                {
                    this.searchingCarriageId = null;
                }
                OnPropertyChanged("SearchingCarriageId");
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
                    RailTransportations = DatabaseConnector.GetRailTransportations().
                    Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                    Where(x => searchingOrderId == null || x.Order.Id == searchingOrderId).Select(x => x).
                    Where(x => searchingCarriageId == null || x.Carriage.Id == searchingCarriageId).Select(x => x).
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
                    searchingOrderId = null;
                    searchingCarriageId = null;
                    searchingStatus = null;
                    RailTransportations = DatabaseConnector.GetRailTransportations();
                    OnPropertyChanged("SearchingId");
                    OnPropertyChanged("SearchingOrderId");
                    OnPropertyChanged("SearchingCarriageId");
                    OnPropertyChanged("SearchingStatus");
                }));
            }
        }

        DefaultCommand? refresh;
        public DefaultCommand Refresh
        {
            get
            {
                return refresh ?? (refresh = new DefaultCommand((obj) =>
                {
                    RailTransportations = DatabaseConnector.GetRailTransportations().
                    Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                    Where(x => searchingOrderId == null || x.Order.Id == searchingOrderId).Select(x => x).
                    Where(x => searchingCarriageId == null || x.Carriage.Id == searchingCarriageId).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).ToList();
                }));
            }
        }

        RailTransportation newRailTransportation = new RailTransportation();
        public RailTransportation NewRailTransportation
        {
            get
            {
                return newRailTransportation;
            }
            set
            {
                newRailTransportation = value;
                OnPropertyChanged("NewRailTransportation");
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

        int orderId;
        public string OrderId
        {
            get
            {
                return orderId.ToString();
            }
            set
            {
                orderId = int.Parse(value);
                OnPropertyChanged("OrderId");
            }
        }

        int carriageId;
        public string CarriageId
        {
            get
            {
                return carriageId.ToString();
            }
            set
            {
                string number = new string(value.TakeWhile(c => Char.IsDigit(c)).ToArray());
                carriageId = int.Parse(number);
                OnPropertyChanged("CarriageId");
            }
        }

        Guid token = new Guid();
        
        public Guid Token
        {
            get
            {
                return token;
            }
            set
            {
                token = value;
                OnPropertyChanged("Token");
            }
        }

        DefaultCommand? refreshToken;
        public DefaultCommand RefreshToken
        {
            get
            {
                return refreshToken ?? (refreshToken = new DefaultCommand((obj) =>
                {
                    Token = Guid.NewGuid();
                }));
            }
        }

        DefaultCommand? addNewRailTransportation;
        public DefaultCommand AddNewRailTransportation
        {
            get
            {
                return addNewRailTransportation ?? (addNewRailTransportation = new DefaultCommand((obj) =>
                {
                    if (carriageId != 0 && orderId != 0 && token != Guid.Empty)
                    {
                        if (DatabaseConnector.CreateNewRailTransportation(orderId, carriageId, token))
                        {
                            NewRailTransportation = new RailTransportation();
                            ErrorMessage = "";
                            MessageBox.Show("Жд перевозка успешно создана");
                            CurrentPage = 0;
                            RailTransportations = DatabaseConnector.GetRailTransportations().
                    Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                    Where(x => searchingOrderId == null || x.Order.Id == searchingOrderId).Select(x => x).
                    Where(x => searchingCarriageId == null || x.Carriage.Id == searchingCarriageId).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).ToList();
                        } else
                        {
                            ErrorMessage = "Ошибка создания новой жд перевозки";
                        }
                    } else
                    {
                        ErrorMessage = "Заполните все поля";
                    }
                }));
            }
        }

        DefaultCommand? closeRailTransportation;
        public DefaultCommand CloseRailTransportation
        {
            get
            {
                return closeRailTransportation ?? (closeRailTransportation = new DefaultCommand((obj) =>
                {
                    if (obj is int railTransportationId)
                    {
                        if (DatabaseConnector.CloseRailTransportation(railTransportationId))
                        {
                            RailTransportations = DatabaseConnector.GetRailTransportations().
                    Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                    Where(x => searchingOrderId == null || x.Order.Id == searchingOrderId).Select(x => x).
                    Where(x => searchingCarriageId == null || x.Carriage.Id == searchingCarriageId).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).ToList();
                        } else
                        {
                            MessageBox.Show("Невозможно закрыть жд перевозку");
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

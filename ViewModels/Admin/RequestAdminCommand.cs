using FreightMate.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FreightMate.ViewModels.Admin
{
    public class RequestAdminCommand : INotifyPropertyChanged
    {
        private List<Request> requests = DatabaseConnector.GetRequestsAdmin();
        public List<Request> Requests
        {
            get
            {
                return requests;
            }
            set
            {
                requests = value;
                OnPropertyChanged("Requests");
            }
        }

        DefaultCommand? refresh;
        public DefaultCommand Refresh
        {
            get
            {
                return refresh ?? (refresh = new DefaultCommand((obj) =>
                {
                    Requests = DatabaseConnector.GetRequestsAdmin().
                                Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                                Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).ToList();
                }));
            }
        }

        DefaultCommand? approveRequest;
        public DefaultCommand ApproveRequest
        {
            get
            {
                return approveRequest ?? (approveRequest = new DefaultCommand((obj) =>
                {
                    if (obj is int requestId)
                    {
                        string stringPrice = Interaction.InputBox("Цена заказа (BYN)", "FreightMate - цена заказа");
                        if (float.TryParse(stringPrice, out float price) && price > 0) {
                            DatabaseConnector.ApproveRequest(requestId, price);
                            Requests = DatabaseConnector.GetRequestsAdmin().
                                Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                                Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).ToList();
                        } else
                        {
                            MessageBox.Show("Неверное значение цены");
                        }
                        
                    }
                }));
            }
        }

        DefaultCommand? denieRequest;
        public DefaultCommand DenieRequest
        {
            get
            {
                return denieRequest ?? (denieRequest = new DefaultCommand((obj) =>
                {
                    if (obj is int requestId)
                    {
                        string? denieMessage = Interaction.InputBox("Причина отклонения", "FreightMate - причина отклонения заявки");
                        denieMessage = denieMessage.Trim();
                        if (denieMessage != "")
                        {
                            DatabaseConnector.DenieRequest(requestId, denieMessage);
                            Requests = DatabaseConnector.GetRequestsAdmin().
                                Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                                Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).ToList();
                        }
                                            }
                }));
            }
        }

        private int? searchingId = null;
        public string SearchingId
        {
            get
            {
                return searchingId == null ? "" : searchingId.ToString() ;
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
                    Requests = DatabaseConnector.GetRequestsAdmin().
                    Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
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
                    Requests = DatabaseConnector.GetRequestsAdmin();
                    OnPropertyChanged("SearchingId");
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

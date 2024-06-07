using FreightMate.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FreightMate.ViewModels.User
{
    public class RequestsCommand : INotifyPropertyChanged
    {
        private List<Request> requests = DatabaseConnector.GetRequests();
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
                    Requests = DatabaseConnector.GetRequests();
                }));
            }
        }

        DefaultCommand? deleteRequest;
        public DefaultCommand DeleteRequest
        {
            get
            {
                return deleteRequest ?? (deleteRequest = new DefaultCommand((obj) =>
                {
                    if (obj is int requestId)
                    {
                        DatabaseConnector.DeleteRequest(requestId);
                        Requests = DatabaseConnector.GetRequests();
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

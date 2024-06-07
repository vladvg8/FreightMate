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
    public class SidebarCommand : INotifyPropertyChanged
    {
        private int _currentPageNumber = 0;
        public int CurrentPageNumber
        {
            get
            {
                return _currentPageNumber;
            }
            set
            {
                _currentPageNumber = value;
                OnPropertyChanged("CurrentPageNumber");
            }
        }

        DefaultCommand? openRequests;
        public DefaultCommand OpenRequests
        {
            get
            {
                return openRequests ?? (openRequests = new DefaultCommand((obj) =>
                {
                    CurrentPageNumber = 1;
                }));
            }
        }

        DefaultCommand? openOrders;
        public DefaultCommand OpenOrders
        {
            get
            {
                return openOrders ?? (openOrders = new DefaultCommand((obj) =>
                {
                    CurrentPageNumber = 2;
                }));
            }
        }

        DefaultCommand? openTransportations;
        public DefaultCommand OpenTransportations
        {
            get
            {
                return openTransportations ?? (openTransportations = new DefaultCommand((obj) =>
                {
                    CurrentPageNumber = 3;
                }));
            }
        }

        DefaultCommand? openUsers;
        public DefaultCommand OpenUsers
        {
            get
            {
                return openUsers ?? (openUsers = new DefaultCommand((obj) =>
                {
                    CurrentPageNumber = 4;
                }));
            }
        }

        DefaultCommand? returnToAuthorization;
        public DefaultCommand ReturnToAuthorization
        {
            get
            {
                return returnToAuthorization ?? (returnToAuthorization = new DefaultCommand((obj) =>
                {
                    AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                    authorizationWindow.Show();
                    Application.Current.Windows[0].Close();
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

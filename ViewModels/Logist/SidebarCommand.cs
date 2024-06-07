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

        DefaultCommand? openOrders;
        public DefaultCommand OpenOrders
        {
            get
            {
                return openOrders ?? (openOrders = new DefaultCommand((obj) =>
                {
                    CurrentPageNumber = 1;
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
                    CurrentPageNumber = 2;
                }));
            }
        }

        DefaultCommand? openRailTransportations;
        public DefaultCommand OpenRailTransportations
        {
            get
            {
                return openRailTransportations ?? (openRailTransportations = new DefaultCommand((obj) =>
                {
                    CurrentPageNumber = 3;
                }));
            }
        }

        DefaultCommand? openCarriages;
        public DefaultCommand OpenCarriages
        {
            get
            {
                return openCarriages ?? (openCarriages = new DefaultCommand((obj) => 
                {
                    CurrentPageNumber = 4;
                }));
            }
        }

        DefaultCommand? openKnowledgesBase;
        public DefaultCommand OpenKnowledgesBase
        {
            get
            {
                return openKnowledgesBase ?? (openKnowledgesBase = new DefaultCommand((obj) =>
                {
                    CurrentPageNumber = 5;
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

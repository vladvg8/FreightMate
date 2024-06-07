using FreightMate.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FreightMate.ViewModels.Logist
{
    public class CarriagesCommand : INotifyPropertyChanged
    {
        List<Carriage> carriages = DatabaseConnector.GetCarriages();
        public List<Carriage> Carriages
        {
            get
            {
                return carriages;
            }
            set
            {
                carriages = value;
                OnPropertyChanged("Carriages");
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

        private string? searchingType = null;
        public string? SearchingType
        {
            get
            {
                return searchingType;
            }
            set
            {
                searchingType = value;
                OnPropertyChanged("SearchingType");
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
                    Carriages = DatabaseConnector.GetCarriages().
                    Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingType) || x.CarriageType.Name == searchingType).Select(x => x).ToList();

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
                    searchingType = null;
                    Carriages = DatabaseConnector.GetCarriages();
                    OnPropertyChanged("SearchingId");
                    OnPropertyChanged("SearchingType");
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
                    Carriages = DatabaseConnector.GetCarriages().
                    Where(x => searchingId == null || x.Id == searchingId).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingStatus) || x.Status == searchingStatus).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingType) || x.CarriageType.Name == searchingType).Select(x => x).ToList();
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

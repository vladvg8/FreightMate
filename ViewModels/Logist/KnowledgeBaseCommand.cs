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
    public class KnowledgeBaseCommand : INotifyPropertyChanged
    {
        List<CarriageType> carriageTypes = DatabaseConnector.GetCarriageTypes();
        public List<CarriageType> CarriageTypes
        {
            get
            {
                return carriageTypes;
            }
            set
            {
                carriageTypes = value;
                OnPropertyChanged("CarriageTypes");
            }
        }

        private float? searchingWeight = null;
        public string SearchingWeight
        {
            get
            {
                return searchingWeight == null ? "" : searchingWeight.ToString();
            }
            set
            {
                if (value != "")
                {
                    if (float.TryParse(value, out float newValue))
                    {
                        if (newValue > 0)
                        {
                            this.searchingWeight = newValue;
                        }
                        else
                        {
                            this.searchingWeight = null;
                        }

                    }
                }
                else
                {
                    this.searchingWeight = null;
                }
                OnPropertyChanged("SearchingWeight");
            }
        }

        private float? searchingVolume = null;
        public string SearchingVolume
        {
            get
            {
                return searchingVolume == null ? "" : searchingVolume.ToString();
            }
            set
            {
                if (value != "")
                {
                    if (float.TryParse(value, out float newValue))
                    {
                        if (newValue > 0)
                        {
                            this.searchingVolume = newValue;
                        }
                        else
                        {
                            this.searchingVolume = null;
                        }
                    }
                }
                else
                {
                    this.searchingVolume = null;
                }
                OnPropertyChanged("SearchingVolume");
            }
        }

       
        DefaultCommand? applyFilter;
        public DefaultCommand ApplyFilter
        {
            get
            {
                return applyFilter ?? (applyFilter = new DefaultCommand((obj) =>
                {
                    CarriageTypes = DatabaseConnector.GetCarriageTypes().
                    Where(x => searchingWeight == null || x.MaxWeight >= searchingWeight).Select(x => x).
                    Where(x => searchingVolume == null || x.MaxVolume >= searchingVolume).Select(x => x).ToList();


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
                    searchingWeight = null;
                    searchingVolume = null;
                    CarriageTypes = DatabaseConnector.GetCarriageTypes();
                    OnPropertyChanged("SearchingWeight");
                    OnPropertyChanged("SearchingVolume");
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

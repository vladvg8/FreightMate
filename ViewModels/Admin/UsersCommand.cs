using System;
using System.Collections.Generic;
using System.ComponentModel;
using FreightMate.Models;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using System.Windows;
using Microsoft.VisualBasic.ApplicationServices;

namespace FreightMate.ViewModels.Admin
{
    public class UsersCommand : INotifyPropertyChanged

    {
        private static List<FreightMate.Models.User> users = DatabaseConnector.GetUsersAdmin();
        public List<FreightMate.Models.User> Users
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
                OnPropertyChanged("Users");
            }
        }

        private int pageNumber = 0;
        public int PageNumber
        {
            get
            {
                return pageNumber;
            }
            set
            {
                pageNumber = value;
                OnPropertyChanged("PageNumber"); 
            }
        }

        DefaultCommand? openHomePage;
        public DefaultCommand? OpenHomePage
        {
            get
            {
                return openHomePage ?? (openHomePage = new DefaultCommand((obj) =>
                {
                    PageNumber = 0;
                }));
            }
        }

        private FreightMate.Models.User newUser = new FreightMate.Models.User();
        public FreightMate.Models.User NewUser
        {
            get
            {
                return newUser;
            }
            set
            {
                newUser = value;
                OnPropertyChanged("NewUser");
            }
        }

        private string errorMessage = "";
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

        DefaultCommand? addUser;
        public DefaultCommand AddUser
        {
            get
            {
                return addUser ?? (addUser = new DefaultCommand((obj) =>
                {
                    if (!NewUser.isEmpty() && NewUser.Role != "")
                    {
                        if (DatabaseConnector.AddUserAdmin(NewUser))
                        {
                            MessageBox.Show("Пользователь успешно добавлен");
                            Users = DatabaseConnector.GetUsersAdmin().
                            Where(x => string.IsNullOrEmpty(searchingLogin) || x.Login.Contains(searchingLogin)).Select(x => x).
                            Where(x => string.IsNullOrEmpty(searchingRole) || x.Role == searchingRole).Select(x => x).ToList();
                            NewUser = new Models.User();
                            PageNumber = 0;
                        } else
                        {
                            ErrorMessage = "Пользователь с таким логином уже существует";
                        }
                    } else
                    {
                        ErrorMessage = "Заполните все поля";
                    }
                }));
            }
        }


        DefaultCommand? deleteUser;
        public DefaultCommand DeleteUser
        {
            get
            {
                return deleteUser ?? (deleteUser = new DefaultCommand((obj) =>
                {
                    if (obj is int userId)
                    {
                        DatabaseConnector.DeleteUser(userId);
                        Users = DatabaseConnector.GetUsersAdmin().
                        Where(x => string.IsNullOrEmpty(searchingLogin) || x.Login.Contains(searchingLogin)).Select(x => x).
                        Where(x => string.IsNullOrEmpty(searchingRole) || x.Role == searchingRole).Select(x => x).ToList();

                    }
                }, (obj) =>
                {
                    if (obj is int userId)
                    {

                        if (Users.Where(x=>x.Id == userId).FirstOrDefault().Role == "ADMIN")
                        {
                            return false;
                        }
                    }
                    return true;
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
                    Users = DatabaseConnector.GetUsersAdmin().
                    Where(x => string.IsNullOrEmpty(searchingLogin) || x.Login.Contains(searchingLogin)).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingRole) || x.Role == searchingRole).Select(x => x).ToList();
                }));
            }
        }

        DefaultCommand? openCreateNewUser;
        public DefaultCommand OpenCreateNewUser
        {
            get
            {
                return openCreateNewUser ?? (openCreateNewUser = new DefaultCommand((obj) =>
                {
                    PageNumber = 1;
                }));
            }
        }

        private string? searchingLogin = null;
        public string? SearchingLogin
        {
            get
            {
                return searchingLogin;
            }
            set
            {
                searchingLogin = value; 
                OnPropertyChanged("SearchingLogin");
            }
        }

        private string? searchingRole = null;
        public string? SearchingRole
        {
            get
            {
                return searchingRole;
            }
            set
            {
                searchingRole = value;
                OnPropertyChanged("SearchingRole");
            }
        }


        DefaultCommand? applyFilter;
        public DefaultCommand ApplyFilter
        {
            get
            {
                return applyFilter ?? (applyFilter = new DefaultCommand((obj) =>
                {
                    Users = DatabaseConnector.GetUsersAdmin().
                    Where(x => string.IsNullOrEmpty(searchingLogin) || x.Login.Contains(searchingLogin)).Select(x => x).
                    Where(x => string.IsNullOrEmpty(searchingRole) || x.Role == searchingRole).Select(x => x).ToList();

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
                    searchingLogin = null;
                    searchingRole = null;
                    Users = DatabaseConnector.GetUsersAdmin();
                    OnPropertyChanged("SearchingLogin");
                    OnPropertyChanged("SearchingRole");
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

using FreightMate.Models;
using FreightMate.Views.Admin;
using FreightMate.Views.Logist;
using FreightMate.Views.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FreightMate.ViewModels.Authorization
{
    public class AuthorizationCommand : INotifyPropertyChanged
    {
        private int _currentPageNumber = 0;


        public int CurrentPageNumber
        {
            get { return _currentPageNumber; }
            set
            {
                _currentPageNumber = value;
                OnPropertyChanged("CurrentPageNumber");
            }
        }

        private DefaultCommand? setRegistrationPage;
        public DefaultCommand? SetRegistrationPage
        {
            get
            {
                return setRegistrationPage ?? (setRegistrationPage = new DefaultCommand((obj) => {
                    CurrentPageNumber = 1;
                    ErrorMessage = "";
                }));
            }
        }

        private DefaultCommand? setAuthenticationPage;
        public DefaultCommand? SetAuthenticationPage
        {
            get
            {
                return setAuthenticationPage ?? (setAuthenticationPage = new DefaultCommand((obj) =>
                {
                    CurrentPageNumber = 0;
                    ErrorMessage = "";
                    OnPropertyChanged("CurrentPageNumber");
                }));
            }
        }

        Models.User? authUser = new Models.User();
        public Models.User? AuthUser
        {
            get
            {
                return authUser;
            }
            set
            {
                authUser = value;
            }
        }
        string errorMessage = "";
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

        DefaultCommand? enter;
        public DefaultCommand? Enter
        {
            get
            {
                return enter ?? (enter = new DefaultCommand((obj) =>
                {
                    if (authUser.isEmpty())
                    {
                        ErrorMessage = "Заполните все поля!";
                    }
                    else
                    {
                        Models.User? user = DatabaseConnector.Authorization(authUser.Login, authUser.Password);
                        if (user == null)
                        {
                            ErrorMessage = "Логин или пароль введены неверно";
                        }
                        else
                        {
                            UserConfig.user = user;
                            if (user.Role == "USER")
                            {
                                UserMainWindow userMainWindow = new UserMainWindow();
                                userMainWindow.Show();
                            }
                            else if (user.Role == "ADMIN")
                            {
                                AdminMainWindow adminMainWindow = new AdminMainWindow();
                                adminMainWindow.Show();
                            }
                            else if (user.Role == "LOGIST")
                            {
                                LogistMainWindow logistMainWindow = new LogistMainWindow();
                                logistMainWindow.Show();
                            }
                            else
                            {
                                MessageBox.Show("Ошибка получения роли пользователя. \nПожалуйста, используйте другую учетную запись или обратитесь к разработчику.");
                            }
                            Application.Current.Windows[0].Close();
                        }
                    }
                }));
            }
        }

        Models.User? regUser = new Models.User();
        public Models.User? ReghUser
        {
            get
            {
                return regUser;
            }
            set
            {
                regUser = value;
            }
        }

        DefaultCommand? register;
        public DefaultCommand? Register
        {
            get
            {
                return register ?? (register = new DefaultCommand((obj) =>
                {
                    if (regUser.isEmpty())
                    {
                        ErrorMessage = "Заполните все поля!";
                    }
                    else
                    {
                        bool result = DatabaseConnector.Registration(regUser.Login, regUser.Password);
                        if (result)
                        {
                            ErrorMessage = "";
                            MessageBox.Show("Вы успешно зарегистрировались!");
                            regUser.Clear();
                            CurrentPageNumber = 0;
                        }
                        else
                        {
                            ErrorMessage = "Логин уже существует";
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

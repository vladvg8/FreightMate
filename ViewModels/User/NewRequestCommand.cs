using FreightMate.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreightMate.ViewModels.User
{
    public class NewRequestCommand : INotifyPropertyChanged
    {
        Request request = new Request();
        public Request Request { get { return request; } set { request = value; } }

        DefaultCommand? insertNewRequest;
        public DefaultCommand InsertNewRequest
        {
            get
            {
                return insertNewRequest ?? (insertNewRequest = new DefaultCommand((obj) =>
                {
                    if (!request.isEmpty())
                    {
                        if (DatabaseConnector.InsertNewRequest(request))
                        {
                            MessageBox.Show("Новый запрос успешно создан");
                        } else
                        {
                            MessageBox.Show("Ошибка создания нового запроса");
                        }
                    } else
                    {
                        MessageBox.Show("Заполните все поля");
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

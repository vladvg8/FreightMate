using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreightMate.Models
{
    public class Request
    {
        public int id;
        public User user;
        public string title;
        public string userName;
        public string userPhoneNumber;
        public string cargoLocationFrom;
        public string cargoLocationTo;
        public string cargoType;
        public float cargoWeight;
        public float cargoVolume;
        public string description;
        public string status;
        public string? deniedMessage;
        public Request()
        {
            this.id = 0;
            this.user = new User();
            this.title = "";
            this.userName = "";
            this.userPhoneNumber = "";
            this.cargoLocationFrom = "";
            this.cargoLocationTo = "";
            this.cargoType = "";
            this.cargoWeight = 0;
            this.cargoVolume = 0;
            this.description = "";
            this.status = "";
            this.deniedMessage = "";
        }

        public Request(int id, User userId, string title, string userName, string userPhoneNumber, string cargoLocationFrom, string cargoLocationTo, string cargoType, float cargoWeight, float cargoVolume, string description, string status, string? deniedMessage)
        {
            this.id = id;
            this.user = userId;
            this.title = title;
            this.userName = userName;
            this.userPhoneNumber = userPhoneNumber;
            this.cargoLocationFrom = cargoLocationFrom;
            this.cargoLocationTo = cargoLocationTo;
            this.cargoType = cargoType;
            this.cargoWeight = cargoWeight;
            this.cargoVolume = cargoVolume;
            this.description = description;
            this.status = status;
            this.deniedMessage = deniedMessage;
        }

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public string Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        public User User
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                if (value.Length <= 50)
                {
                    this.title = value;
                } else
                {
                    MessageBox.Show("Название заявки не может быть длинее 50 символов");
                }
            }
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                if (value.Length <= 20)
                {
                    this.userName = value;
                } else
                {
                    MessageBox.Show("Имя не можеть быть длинее 20 символов");
                }
            }
        }

        public string UserPhoneNumber
        {
            get
            {
                return this.userPhoneNumber;
            }
            set
            {
                Regex regex = new Regex(@"^\+375\d{2}\d{3}\d{2}\d{2}$");
                if (regex.IsMatch(value))
                {
                    this.userPhoneNumber = value;
                } else
                {
                    MessageBox.Show("Номер телефона должен быть в формате +375ххххххххх");
                }
            }
        }
        public string CargoLocationFrom
        {
            get
            {
                return this.cargoLocationFrom;
            }
            set
            {
                if (value.Length <= 100)
                {
                    this.cargoLocationFrom = value;
                } else
                {
                    MessageBox.Show("Значение поля 'Местоположение груза' не может быть длиннее 100 символов");
                }
            }
        }

        public string CargoLocationTo
        {
            get
            {
                return this.cargoLocationTo;
            }
            set
            {
                if (value.Length <= 100)
                {
                    this.cargoLocationTo = value;
                }
                else
                {
                    MessageBox.Show("Значение поля 'Место доставки' не может быть длиннее 100 символов");
                }
            }
        }

        public string CargoType
        {
            get
            {
                return this.cargoType;
            }
            set
            {
                if (value.Length <= 64)
                {
                    this.cargoType = value;
                } else
                {
                    MessageBox.Show("Значение поля 'Тип груза' не может быть длиннее 64 символов");
                }
            }
        }

        public string CargoWeight
        {
            get
            {
                return this.cargoWeight.ToString();
            }
            set
            {
                if (float.TryParse(value, out float newValue))
                {
                    if (newValue > 0 && newValue <= 120000)
                    {
                        this.cargoWeight = newValue;
                    } else
                    {
                        MessageBox.Show("Вес груза может быть не больше 120000");
                    }
                } else
                {
                    MessageBox.Show("Неверное значение");
                }
            }
        }

        public string CargoVolume
        {
            get
            {
                return this.cargoVolume.ToString();
            }
            set
            {
                if (float.TryParse(value, out float newValue))
                {
                    if (newValue > 0 && newValue <= 144)
                    {
                        this.cargoVolume = newValue;
                    }
                    else
                    {
                        MessageBox.Show("Объем груза может быть не больше 144");
                    }
                } else
                {
                    MessageBox.Show("Неверное значение");
                }
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                if (value.Length <= 300)
                {
                    this.description = value;
                } else
                {
                    MessageBox.Show("Значение поля 'Описание заявки' не может быть длиннее 300 символов");
                }
            }
        }

        public string? DeniedMessage { get { return this.deniedMessage; } set { this.deniedMessage = value; } }

        public bool isEmpty()
        {
            bool result = false;

            if (this.title == "" || this.userName == "" || this.userPhoneNumber == "" || this.cargoType ==  "" || this.cargoLocationFrom == "" || 
                this.CargoLocationTo == "" || this.cargoWeight == 0 || this.cargoVolume == 0 || this.Description == "")
            {
                result = true;
            }

            return result;
        }
    }
}

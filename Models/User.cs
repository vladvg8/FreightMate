using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightMate.Models
{
    public class User
    {
        private int _id;
        private string _login;
        private string _password;
        private string _role;
        public User(int id, string login, string password, string role)
        {
            this._id = id;
            _login = login;
            _password = password;
            _role = role;
        }
        public User()
        {
            this._login = "";
            this._password = "";
            this._role = "";
        }
        
        public int Id {  get { return this._id; } set { this._id = value; } }
        public string Login { get { return this._login; } set { this._login = value; } }
        public string Password { get { return this._password; } set { this._password = value; } }
        public string Role { get { return this._role; } set { this._role = value; } }

        public bool isEmpty()
        {
            return this._login == "" || this._password == "";
        }
        public void Clear()
        {
            this._login = "";
            this._password = "";
            this._role = "";
        }
    }
}

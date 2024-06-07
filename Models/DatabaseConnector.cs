using Azure.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FreightMate.Models
{
    public static class DatabaseConnector
    {
        private static readonly DatabaseContext db = new DatabaseContext();

        #region AuthorizationConnector

        public static User? Authorization(string login, string password)
        {
            User? user = null;
            try
            {
                var users = db.Users.AsNoTracking();
                foreach (User u in users)
                {
                    if (u.Login == login && u.Password == password)
                    {
                        user = u;
                        break;
                    }
                } 
            } catch (Exception ex)
            {

            }
            return user;
        }
        public static bool Registration(string login, string password)
        {
            bool result = false;
            try
            {
                var existingUser = db.Users.AsNoTracking().FirstOrDefault(u => u.Login == login);
                if (existingUser != null)
                {
                    return false;
                }
                db.Users.Add(new User() { Login = login, Password = password, Role = "USER" });
                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        #endregion

        #region UserConnector

        public static bool InsertNewRequest(Request request)
        {
            try
            {
                try
                {
                    db.Attach(UserConfig.user);
                }
                catch { }
                db.Requests.Add(new Request() { User = UserConfig.user, Status = "Ожидает обработки", Title = request.Title, UserName = request.UserName, 
                    UserPhoneNumber = request.UserPhoneNumber, CargoLocationFrom = request.CargoLocationFrom, 
                    CargoLocationTo = request.CargoLocationTo, CargoType = request.CargoType, CargoWeight = request.CargoWeight, 
                    CargoVolume = request.CargoVolume, Description = request.Description, DeniedMessage = null });
                db.SaveChanges();
                return true;
            } catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
                Clipboard.SetText(ex.InnerException.Message);
            }
            return false;
        }

        public static List<Request> GetRequests()
        {
            List<Request> result = new List<Request>();
            try
            {
                var requests = db.Requests.AsNoTracking().Where(x => x.User == UserConfig.user).Select(x=>x);
                foreach (Request request in requests)
                {
                    result.Add(request);
                }
            } catch (Exception ex)
            {

            }
            return result;
        }

        public static List<Order> GetOrders()
        {
            List<Order> result = new List<Order>();
            try
            {
                try
                {
                    db.Attach(UserConfig.user);
                }
                catch { }
                var orders = db.Orders.AsNoTracking().Include(x => x.Request).Where(x => x.Request.User == UserConfig.user).Select(x => x);
                foreach (Order order in orders)
                {
                    result.Add(order);
                }
            } catch (Exception ex)
            {

            }
            return result;
        }

        public static void DeleteRequest(int requestId)
        {
            try
            {
                var deletingRequest = db.Requests.AsNoTracking().Where(x => x.Id == requestId).Include(x => x.User).FirstOrDefault();
                try
                {
                    db.Attach(deletingRequest);
                } catch { }
                if (deletingRequest != null && (deletingRequest.Status == "Отклонен" || deletingRequest.Status == "Ожидает обработки"))
                {
                    db.Requests.Remove(deletingRequest);
                    db.SaveChanges();
                }
            } catch (Exception ex)
            {

            }
        }

        public static void DeleteOrder(int orderId)
        {
            try
            { 
                var deletingOrder = db.Orders.AsNoTracking().Where(x => x.Id == orderId).Include(x => x.Request).Include(x=>x.Request.User).FirstOrDefault();
                try
                {
                    db.Attach(deletingOrder);
                } catch { }
                if (deletingOrder != null && deletingOrder.Status == "Удален")
                {
                    db.Orders.Remove(deletingOrder);
                    db.SaveChanges();
                    
                }
            } catch (Exception ex)
            {

            }
        }

        public static void SetDeliveryTimeOrder(int orderId, DateTime time)
        {
            try
            {
                var currentOrder = db.Orders.Where(x => x.Id == orderId).Include(x => x.Request).Include(x => x.Request.User).FirstOrDefault();
                try
                {
                    db.Attach(currentOrder);
                }
                catch { }
                if (currentOrder != null && currentOrder.Status == "Выбор времени приема груза")
                {
                    currentOrder.Status = "На перевозке";
                    db.SaveChanges();
                }
                var currentTransportation = db.Transportations.Where(x => x.Order.Id == orderId).Include(x => x.Order).Include(x => x.Order.Request).Include(x => x.Order.Request.User).FirstOrDefault();
                try
                {
                    db.Attach(currentTransportation);
                } catch { }
                if (currentTransportation != null && currentTransportation.Status == "Выбор времени приема груза")
                {
                    currentTransportation.Status = "Активна";
                    currentTransportation.GetingCargoTime = time;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region AdminConnector

        public static List<Request> GetRequestsAdmin()
        {
            List<Request> result = new List<Request>();
            try
            {
                var requests = db.Requests.AsNoTracking();
                foreach(Request request in requests)
                {
                    result.Add(request);
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public static void ApproveRequest(int requestId, float price)
        {
            try
            {
                Request? updationRequest = db.Requests.Where(x => x.Id == requestId).Include(x => x.User).FirstOrDefault();
                if (updationRequest != null && updationRequest.Status == "Ожидает обработки")
                {
                    updationRequest.Status = "Одобрен";
                    try
                    {
                        db.Attach(updationRequest);
                    }
                    catch { }
                    db.Orders.Add(new Order() { Request = updationRequest, Price = price.ToString(), Status = "Ожидает обработки логистом", DeniedMessage = null, ExpectedDeliveryTime = null });
                    db.SaveChanges();
                } else
                {
                    MessageBox.Show("Простите, данную заявку невозможно одобрить.\nВозможные причины:\n1) Заявки больше не существует\n2) Заявка была уже изменена\n\nПрошу вас обновить список заявок");
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void DenieRequest(int requestId, string deniedMessage)
        {
            try
            { 
                Request? updationRequest = db.Requests.Where(x => x.Id == requestId).Include(x => x.User).FirstOrDefault();
                if (updationRequest != null && updationRequest.Status == "Ожидает обработки")
                {
                    updationRequest.Status = "Отклонен";
                    updationRequest.DeniedMessage = deniedMessage;
                    try
                    {
                        db.Attach(updationRequest);
                    }
                    catch { }
                    db.SaveChanges();  
                }
                else
                {
                    MessageBox.Show("Простите, данную заявку невозможно отклонить.\nВозможные причины:\n1) Заявки больше не существует\n2) Заявка была уже изменена\n\nПрошу вас обновить список заявок");
                }

            } catch (Exception ex)
            {

            }
        }

        public static List<User> GetUsersAdmin()
        {
            try
            {
                List<User> users = new List<User>();
                var dbUsers = db.Users.AsNoTracking();
                foreach (User user in dbUsers)
                {
                    users.Add(user);
                }
                return users;
            } catch (Exception ex)
            {

            }
            return new List<User>();
        }

        public static bool AddUserAdmin(User user)
        {
            try
            {
                var existingUser = db.Users.AsNoTracking().FirstOrDefault(u => u.Login == user.Login);
                if (existingUser != null)
                {
                    return false;
                } else
                {
                    db.Users.Add(new User() { Login = user.Login, Password = user.Password, Role = user.Role });
                    db.SaveChanges();
                }
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }

        public static List<Order> GetOrdersAdmin()
        {
            List<Order> result = new List<Order>();
            try
            {
                var orders = db.Orders.AsNoTracking().Include(x => x.Request);
                foreach (Order order in orders)
                {
                    result.Add(order);
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public static void DeleteUser(int userId)
        {
            try
            {
                var deletingUser = db.Users.AsNoTracking().Where(x => x.Id == userId).FirstOrDefault();
                if (deletingUser != null)
                {
                    try
                    {
                        db.Attach(deletingUser);
                    } catch { }
                    db.Users.Remove(deletingUser);
                    db.SaveChanges();
                }
            } catch (Exception ex)
            {
                
            }
        }

        public static List<Transportation> GetTransportationsAdmin()
        {
            List<Transportation> result = new List<Transportation>();
            try
            {
                var transportations = db.Transportations.AsNoTracking().Include(x => x.Order);
                foreach(Transportation transportation in transportations)
                {
                    result.Add(transportation);
                }
            } catch (Exception ex)
            {

            }
            return result;
        }

        #endregion

        #region LogistConnector

        public static void ApproveOrder(int orderId, DateTime time)
        {
            try
            {
                Order? approvingOrder = db.Orders.Where(x => x.Id == orderId).Include(x => x.Request).Include(x => x.Request.User).FirstOrDefault();
                if (approvingOrder != null && approvingOrder.Status == "Ожидает обработки логистом")
                {
                    approvingOrder.Status = "Активен";
                    approvingOrder.ExpectedDeliveryTime = time;
                    try
                    {
                        db.Attach(approvingOrder);
                    }
                    catch { }
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Простите, данный заказ невозможно одобрить.\nВозможные причины:\n1) Заказа больше не существует\n2) Заказ уже был изменен\n\nПрошу вас обновить список заказов");
                }
            } catch (Exception ex)
            {

            }
        }

        public static void DenieOrder(int orderId, string deningMessage)
        {
            try
            {
                Order? deningOrder = db.Orders.Where(x => x.Id == orderId).Include(x => x.Request).Include(x => x.Request.User).FirstOrDefault();
                if (deningOrder != null && deningOrder.Status == "Ожидает обработки логистом")
                {
                    deningOrder.Status = "Удален";
                    deningOrder.DeniedMessage = deningMessage;
                    try
                    {
                        db.Attach(deningOrder);
                    } catch { }
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Простите, данный заказ невозможно отклонить.\nВозможные причины:\n1) Заказа больше не существует\n2) Заказ уже был изменен\n\nПрошу вас обновить список заказов");
                }

            } catch (Exception ex)
            {

            }
        }

        public static List<CarriageType> GetCarriageTypes()
        {
            List<CarriageType> result = new List<CarriageType>();
            try
            {
                var carriageTypes = db.CarriageTypes.AsNoTracking();
                foreach(CarriageType carriageType in carriageTypes) 
                {
                    result.Add(carriageType);
                }
            } catch (Exception ex)
            {

            }
            return result;
        }

        public static List<Carriage> GetCarriages()
        {
            List<Carriage> result = new List<Carriage>();
            try
            {
                var carriages = db.Carriages.AsNoTracking().Include(x=>x.CarriageType);
                foreach(Carriage carriage in carriages)
                {
                    result.Add(carriage);
                }
            } catch (Exception ex)
            {

            }
            return result;
        }

        public static bool AddNewTransportation(string driverName, string warehouseLocation, int orderId)
        {
            bool result = false;
            try
            {
                Order? order = db.Orders.Where(x => x.Id ==  orderId).FirstOrDefault();
                if (order != null && order.Status == "Активен")
                {
                    try
                    {
                        db.Attach(order);
                    }
                    catch { }
                    order.Status = "Выбор времени приема груза";
                    db.Transportations.Add(new Transportation() { Order = order, DriverName = driverName, WarehouseLocation = warehouseLocation, Status = "Выбор времени приема груза" });
                    db.SaveChanges();
                    result = true;
                }
            } catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public static bool CloseTransportation(int transportationId)
        {
            bool result = false;
            try
            {
                Transportation? transportation = db.Transportations.Where(x => x.Id == transportationId).Include(x=>x.Order).Include(x=>x.Order.Request).Include(x=>x.Order.Request.User).FirstOrDefault();
                if (transportation != null && transportation.Status == "Активна")
                {
                    try
                    {
                        db.Attach(transportation);
                    }
                    catch { }
                    transportation.Status = "Завершена";
                    db.SaveChanges();
                    Order transOrder = transportation.Order;
                    Order order = db.Orders.Where(x => x.Id == transOrder.Id).Include(x => x.Request).Include(x => x.Request.User).FirstOrDefault();
                    if (order != null && order.Status == "На перевозке")
                    {
                        try
                        {
                            db.Attach(order);
                        } catch { }
                        order.Status = "На складе";
                        db.SaveChanges();
                        result = true;
                    }
                }
            } catch (Exception ex)
            {

            }
            return result;
        }

        public static List<RailTransportation> GetRailTransportations()
        {
            List<RailTransportation> result = new List<RailTransportation>();
            try
            {
                foreach(RailTransportation railTransportation in db.RailTransportations.AsNoTracking().Include(x=>x.Carriage).Include(x=>x.Carriage.CarriageType).Include(x=>x.Order).Include(x=>x.Order))
                {
                    result.Add(railTransportation);
                }
            } catch (Exception ex)
            {

            }
            return result;
        }

        public static bool CreateNewRailTransportation(int orderId, int carriageId, Guid token)
        {
            bool result = false;
            try
            {
                Order? order = db.Orders.Where(x => x.Id ==  orderId).FirstOrDefault();
                Carriage? carriage = db.Carriages.Where(x => x.Id ==  carriageId).FirstOrDefault();
                if (order != null && order.Status == "На складе" && carriage != null && carriage.Status == "Свободен")
                {
                    try
                    {
                        db.Attach(order);
                    } catch { }
                    order.Status = "На жд перевозке";
                    try
                    {
                        db.Attach(carriage);
                    } catch { }
                    carriage.Status = "Занят";
                    carriage.RailTransportationToken = token;
                    db.RailTransportations.Add(new RailTransportation() { Order =  order, Carriage = carriage, Token = token, Status = "Активна" });
                    db.SaveChanges();
                    result = true;
                }
            } catch (Exception ex)
            {

            }
            return result;
        }

        public static bool CloseRailTransportation(int railTransportationId)
        {
            bool result = false;
            try
            {
                RailTransportation? railTransportation = db.RailTransportations.Where(x => x.Id == railTransportationId).Include(x => x.Order).Include(x => x.Carriage).FirstOrDefault();
                if (railTransportation != null && railTransportation.Status == "Активна")
                {
                    int orderId = railTransportation.Order.Id;
                    int carriageId = railTransportation.Carriage.Id;
                    Order? order = db.Orders.Where(x => x.Id == orderId).FirstOrDefault();
                    Carriage? carriage = db.Carriages.Where(x => x.Id == carriageId).FirstOrDefault();
                    if (order != null && order.Status == "На жд перевозке" && carriage != null && carriage.Status == "Занят")
                    {
                        try
                        {
                            db.Attach(order);
                        }
                        catch { };
                        order.Status = "Завершен";
                        try
                        {
                            db.Attach(carriage);
                        }
                        catch { }
                        carriage.Status = "Свободен";
                        carriage.RailTransportationToken = Guid.Empty;
                        try
                        {
                            db.Attach(railTransportation);
                        }
                        catch { }
                        railTransportation.Status = "Завершена";
                        db.SaveChanges();
                        result = true;
                    }
                }

            } catch(Exception ex)
            {

            }
            return result;
        }


        #endregion
    }
}

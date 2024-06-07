using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightMate.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            Database.EnsureCreated();
            SeedData();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<Carriage> Carriages { get; set; }
        public DbSet<CarriageType> CarriageTypes { get; set; }
        public DbSet<RailTransportation> RailTransportations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasKey(f => f.Id);
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();

            modelBuilder.Entity<Request>().HasKey(f => f.Id);
            modelBuilder.Entity<Order>().HasKey(f => f.Id);
            modelBuilder.Entity<Transportation>().HasKey(f => f.Id);
            modelBuilder.Entity<Carriage>().HasKey(f => f.Id);
            modelBuilder.Entity<CarriageType>().HasKey(f => f.Id);
            modelBuilder.Entity<RailTransportation>().HasKey(f => f.Id);
        }

        private void SeedData() 
        {
            if (!Users.Any())
            {
                Users.Add(new User() { Login = "admin", Password = "admin", Role = "ADMIN" });
                Users.Add(new User() { Login = "logist", Password = "logist", Role = "LOGIST" });
                SaveChanges();
            }   
            if (!CarriageTypes.Any())
            {
                CarriageTypes.Add(new CarriageType()
                {
                    Name = "Крытый грузовой вагон",
                    MaxWeight = 68000,
                    MaxVolume = 138,
                    Description =
                    "Крытые вагоны предназначены для грузоперевозки ценных грузов и грузов, требующих защиты от атмосферных воздействий. " +
                    "В них перевозят зерно, тарные и искусственные грузы и ряд других.",
                    PhotoUrl = @"..\..\Resources\Images\CarrigesContent\Boxcar.jpg"
                });
                CarriageTypes.Add(new CarriageType()
                {
                    Name = "Полувагон",
                    MaxWeight = 71000,
                    MaxVolume = 75,
                    Description =
                    "Полувагоны предназначены для перевозки сыпучих грузов, не требующих защиты от атмосферных осадков – " +
                    "руды, леса, уголь, металл, а также автомашины, сельскохозяйственная техника и т.д.",
                    PhotoUrl = @"..\..\Resources\Images\CarrigesContent\GondolaCar.png"
                });
                CarriageTypes.Add(new CarriageType()
                {
                    Name = "Хоппер",
                    MaxWeight = 70000,
                    MaxVolume = 81,
                    Description = "Разновидность полувагона, используемая для массовых перевозок удобрений, цемента, зерна и других сыпучих грузов. " +
                    "Закрытые используются для грузов, которые нужно защищать от атмосферных осадков. " +
                    "Открытые хопперы используют для перевозки горячего агломерата и окатышей, угля, торфа, кокса.",
                    PhotoUrl = @"..\..\Resources\Images\CarrigesContent\Hopper.jpg"
                });
                CarriageTypes.Add(new CarriageType()
                {
                    Name = "Платформа",
                    MaxWeight = 71000,
                    MaxVolume = 144,
                    Description = "Платформы предназначены для перевозки длинномерных грузов (рельсов, лесоматериалов), " +
                    "контейнеров, автомобилей и различных автодорожных и сельскохозяйственных машин.Контейнерные платформы не " +
                    "имеют бортов и оборудованы специальными замками для крепления большегрузных универсальных контейнеров любых типов." +
                    " Платформы для перевозки леса имеют торцевые стены и дополнительные специальные стойки, предотвращающие смещение груза.",
                    PhotoUrl = @"..\..\Resources\Images\CarrigesContent\Platform.jpg"
                });
                CarriageTypes.Add(new CarriageType()
                {
                    Name = "Вагон-цистерна",
                    MaxWeight = 67000,
                    MaxVolume = 83,
                    Description = "В цистернах перевозят жидкие грузы, нефтепродукты. " +
                    "В зависимости от типа перевозимого нефтепродукта цистерны обеспечивают приборами для верхнего или нижнего слива. " +
                    "Имеются цистерны для перевозки вязких нефтепродуктов (мазута, смазочных масел), котлы которых имеют обогревательные рубашки. " +
                    "К этой группе относятся и цистерны для перевозки молока, котлы изготовлены из нержавеющей стали и снабжены теплоизоляционным слоем. " +
                    "Имеются цистерны для перевозки спирта, кислот и других грузов.",
                    PhotoUrl = @"..\..\Resources\Images\CarrigesContent\Tank.png"
                });
                CarriageTypes.Add(new CarriageType()
                {
                    Name = "Изотермический вагон",
                    MaxWeight = 50000,
                    MaxVolume = 117,
                    Description = "крытый грузовой вагон для перевозки скоропортящихся грузов. " +
                    "Кузов изотермического вагона для уменьшения тепловых потерь снабжён теплоизоляцией из полистирола," +
                    " пенополиуретана и других материалов, имеет приспособления для рационального размещения груза.",
                    PhotoUrl = @"..\..\Resources\Images\CarrigesContent\IsothermalCar.jpg"
                });
                CarriageTypes.Add(new CarriageType()
                {
                    Name = "Вагон-транспортер",
                    MaxWeight = 120000,
                    MaxVolume = 90,
                    Description = "Бывают разной грузоподъемности, эти вагоны строят для перевозки тяжеловесных и " +
                    "крупногабаритных грузов (роторы, генераторы, турбины, трансформаторы и др.).",
                    PhotoUrl = @"..\..\Resources\Images\CarrigesContent\TransporterCar.jpg"
                });
                SaveChanges();
            }
            if (!Carriages.Any())
            {
                for(int i = 0; i < 10;i++)
                {
                    CarriageType? type = CarriageTypes.Where(x => x.Id == 1).FirstOrDefault();
                    try
                    {
                        Attach(type);
                    }
                    catch { }
                    if (type != null)
                    {
                        Carriages.Add(new Carriage() { CarriageType = type, RailTransportationToken = new Guid(), Status = "Свободен" });
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    CarriageType? type = CarriageTypes.Where(x => x.Id == 2).FirstOrDefault();
                    try
                    {
                        Attach(type);
                    }
                    catch { }
                    if (type != null)
                    {
                        Carriages.Add(new Carriage() { CarriageType = type, RailTransportationToken = new Guid(), Status = "Свободен" });
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    CarriageType? type = CarriageTypes.Where(x => x.Id == 3).FirstOrDefault();
                    try
                    {
                        Attach(type);
                    }
                    catch { }
                    if (type != null)
                    {
                        Carriages.Add(new Carriage() { CarriageType = type, RailTransportationToken = new Guid(), Status = "Свободен" });
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    CarriageType? type = CarriageTypes.Where(x => x.Id == 4).FirstOrDefault();
                    try
                    {
                        Attach(type);
                    }
                    catch { }
                    if (type != null)
                    {
                        Carriages.Add(new Carriage() { CarriageType = type, RailTransportationToken = new Guid(), Status = "Свободен" });
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    CarriageType? type = CarriageTypes.Where(x => x.Id == 5).FirstOrDefault();
                    try
                    {
                        Attach(type);
                    }
                    catch { }
                    if (type != null)
                    {
                        Carriages.Add(new Carriage() { CarriageType = type, RailTransportationToken = new Guid(), Status = "Свободен" });
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    CarriageType? type = CarriageTypes.Where(x => x.Id == 6).FirstOrDefault();
                    try
                    {
                        Attach(type);
                    }
                    catch { }
                    if (type != null)
                    {
                        Carriages.Add(new Carriage() { CarriageType = type, RailTransportationToken = new Guid(), Status = "Свободен" });
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    CarriageType? type = CarriageTypes.Where(x => x.Id == 6).FirstOrDefault();
                    try
                    {
                        Attach(type);
                    }
                    catch { }
                    if (type != null)
                    {
                        Carriages.Add(new Carriage() { CarriageType = type, RailTransportationToken = new Guid(), Status = "Свободен" });
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    CarriageType? type = CarriageTypes.Where(x => x.Id == 7).FirstOrDefault();
                    try
                    {
                        Attach(type);
                    }
                    catch { }
                    if (type != null)
                    {
                        Carriages.Add(new Carriage() { CarriageType = type, RailTransportationToken = new Guid(), Status = "Свободен" });
                    }
                }
                SaveChanges();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path  = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FreightMate";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);            
            }
            optionsBuilder.UseSqlite("Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FreightMate\\FreightMate.db");
        }
    }
}

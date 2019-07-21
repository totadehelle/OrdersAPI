using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Xml;
using OrdersManagement.Models;

namespace OrdersManagement.Database
{
    public sealed class ApplicationContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<IpAddress> IpAddresses { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasIndex(u => u.OrderId);
            modelBuilder.Entity<Order>().HasIndex(u => u.CustomerName);
            modelBuilder.Entity<Order>().HasIndex(u => u.OrderIp);
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
            //DbFiller();
        }

        
        //Function for filling the database with some test data
        public void DbFiller()
        {
            /*var names = new []{"Tom", "John", "Sophie", "Anna"};
            var phones = new[] {"phone1", "phone2", "phone3", "phone4"};
            var emails = new[] {"email1", "email2", "email3", "email4"};
            var addresses = new[] {"address1", "address2", "address3", "address4"};
            var comments = new[] {null, "comment1", "comment2", "comment3"};
            var Ips = new[] {"0.0.0.0", "0.0.0.1", "0.0.1.2", "1.1.1.1"};
                
            var random = new Random();

            Console.WriteLine("Filling");
            
            OrdersModel model = new OrdersModel(this);
                
                for (var i = 0; i < 2; i++)
                {
                    var order = new Order()
                    {
                        OrderId = i, 
                        CustomerName = names[random.Next(4)],
                        Address = addresses[random.Next(4)],
                        Phone = phones[random.Next(4)],
                        Email = emails[random.Next(4)],
                        Comment = comments[random.Next(4)],
                        DeliveryDate = DateTime.Today,
                        OfferAccepted = true,
                        OrderIp = Ips[random.Next(4)]
                    };
                    
                    this.Orders.Add(order);
                }

                this.SaveChanges();*/

                var IPs = this.Orders.Select(e => e.OrderIp).Distinct().ToList();
                foreach (var ip in IPs)
                {
                    var ipAddress = new IpAddress();
                    ipAddress.Ip = ip;
                    ipAddress.OrdersCount = this.Orders.Count(e => e.OrderIp == ip);
                    this.IpAddresses.Add(ipAddress);
                }

                this.SaveChanges();
                
                Console.WriteLine("Filled");
            }
        
    }
}
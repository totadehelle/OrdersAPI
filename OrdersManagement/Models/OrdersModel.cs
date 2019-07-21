using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersManagement.Database;

namespace OrdersManagement.Models
{
    public class OrdersModel
    {
        private readonly ApplicationContext _context;

        public OrdersModel(ApplicationContext context)
        {
            _context = context;
        }
        
        public async Task<ActionResult<IEnumerable<Response>>> GetOrders()
        {
            var orders = await _context.Orders.Take(25).AsNoTracking().ToListAsync();
            var ordersWithIpCount = new List<Response>();
            foreach (var order in orders)
            {
                var response = new Response
                {
                    order = order,
                    IpCount = _context.IpAddresses.AsNoTracking().First(e => e.Ip == order.OrderIp).OrdersCount
                };
                ordersWithIpCount.Add(response);
            }
            return ordersWithIpCount;
        }

        public async Task<ActionResult<Response>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            return new Response
            {
                order = order,
                IpCount = _context.IpAddresses.AsNoTracking().First(e => e.Ip == order.OrderIp).OrdersCount
            };
        }

        public async Task CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            
            AddOrEditIpAddress(order.OrderIp);
            
            await _context.SaveChangesAsync();
        } 
        
        public async Task<bool> EditOrder(int id, Order order)
        {
            var oldOrderIp = _context.Orders.Find(id).OrderIp;
            var newOrderIp = order.OrderIp;
            
            _context.Entry(order).State = EntityState.Modified;

            if (oldOrderIp != newOrderIp)
            {
                AddOrEditIpAddress(oldOrderIp);
                AddOrEditIpAddress(newOrderIp);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);
            AddOrEditIpAddress(order.OrderIp);
            await _context.SaveChangesAsync();

            return true;
        }
        
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        private bool IpExists(string ip)
        {
            return _context.IpAddresses.Any(e => e.Ip == ip);
        }

        private void AddOrEditIpAddress(string orderIp)
        {
            IpAddress ipAddress = new IpAddress();
            ipAddress.Ip = orderIp;
            ipAddress.OrdersCount = _context.Orders.Count(e => e.OrderIp == orderIp);
            
            if (IpExists(ipAddress.Ip))
            {
                _context.Entry(ipAddress).State = EntityState.Modified;
            }
            else
            {
                _context.IpAddresses.Add(ipAddress);
            }
        }
    }
}
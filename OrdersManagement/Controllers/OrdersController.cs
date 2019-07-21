using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersManagement.Database;
using OrdersManagement.Models;

namespace OrdersManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly OrdersModel _model;

        public OrdersController(ApplicationContext context)
        {
            _context = context;
            _model = new OrdersModel(_context);
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Response>>> GetOrders()
        {
            var orders = await _model.GetOrders();
            return new JsonResult(orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> GetOrder(int id)
        {
            var order = await _model.GetOrder(id);

            if (order == null)
            {
                return NotFound();
            }

            return new JsonResult(order);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            var isSuccessful = await _model.EditOrder(id, order); 
            
            if(isSuccessful) return NoContent();

            return NotFound();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            await _model.CreateOrder(order);

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var isSuccessful = await _model.DeleteOrder(id);
            if (!isSuccessful)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

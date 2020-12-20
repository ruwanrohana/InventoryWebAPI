using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryWebAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace InventoryWebAPI.Controllers
{
    [Authorize]
    [Route("api/Items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly InventoryContext _context;

        public ItemsController(InventoryContext context)
        {
            _context = context;
        }

        #region GetAllItems        
        // GET: api/Items
        [HttpGet("GetAllItems")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            try
            {
                return await _context.Items.ToListAsync();
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
        #endregion

        #region GetItem
        // GET: api/Items/5
        [HttpGet("GetItem/{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            try
            {
                var item = await _context.Items.FindAsync(id);

                if (item == null)
                {
                    return NotFound();
                }

                return item;
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }            
            
        }
        #endregion

        #region EditItem
        // PUT: api/Items/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Item item)
        {
            if (id != item.ItemId)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        #endregion

        #region SaveItem
        // POST: api/Items
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            try
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetItem", new { id = item.ItemId }, item);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
           
        }
        #endregion

        #region DeleteItem
        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(int id)
        {
            try
            {
                var item = await _context.Items.FindAsync(id);
                if (item == null)
                {
                    return NotFound();
                }

                _context.Items.Remove(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {

                return NotFound(ex);
            }           
           
        }
        #endregion

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }
    }
}

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
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceItemsController : ControllerBase
    {
        private readonly InventoryContext _context;

        public InvoiceItemsController(InventoryContext context)
        {
            _context = context;
        }

        #region GetAllInvoiceItems       
        // GET: api/InvoiceItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceItem>>> GetInvoiceItems()
        {
            try
            {
                return await _context.InvoiceItems.ToListAsync();
            }
            catch (Exception ex)
            {

                return NotFound(ex);
            }            
            
        }
        #endregion

        #region GetInvoiceItem  
        // GET: api/InvoiceItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceItem>> GetInvoiceItem(int id)
        {

            try
            {
                var invoiceItem = await _context.InvoiceItems.FindAsync(id);

                if (invoiceItem == null)
                {
                    return NotFound();
                }

                return invoiceItem;
            }
            catch (Exception ex)
            {

                return NotFound(ex);
            }            
        }
        #endregion

        #region EditInvoiceItem  
        // PUT: api/InvoiceItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoiceItem(int id, InvoiceItem invoiceItem)
        {
            if (id != invoiceItem.InvoiceItemId)
            {
                return BadRequest();
            }

            _context.Entry(invoiceItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceItemExists(id))
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

        #region SaveInvoiceItem  
        // POST: api/InvoiceItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<InvoiceItem>> PostInvoiceItem(InvoiceItem invoiceItem)
        {
            try
            {
                _context.InvoiceItems.Add(invoiceItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetInvoiceItem", new { id = invoiceItem.InvoiceItemId }, invoiceItem);
            }
            catch (Exception ex)
            {

                return NotFound(ex);
            }                 
          
        }
        #endregion

        #region DeleteInvoiceItem
        // DELETE: api/InvoiceItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InvoiceItem>> DeleteInvoiceItem(int id)
        {
            try
            {
                var invoiceItem = await _context.InvoiceItems.FindAsync(id);
                if (invoiceItem == null)
                {
                    return NotFound();
                }

                _context.InvoiceItems.Remove(invoiceItem);
                await _context.SaveChangesAsync();

                return invoiceItem;
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }           
        }
        #endregion

        private bool InvoiceItemExists(int id)
        {
            return _context.InvoiceItems.Any(e => e.InvoiceItemId == id);
        }
    }
}

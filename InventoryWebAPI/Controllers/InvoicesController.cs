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
    [Route("api/Invoices")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly InventoryContext _context;

        public InvoicesController(InventoryContext context)
        {
            _context = context;
        }

        #region GetAllInvoices
        // GET: api/Invoices
        [HttpGet("GetAllInvoices")]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            try
            {
                var invoice = _context.Invoices
               .Include(inv => inv.InvoiceItems)
                  .ThenInclude(InvoiceItem => InvoiceItem.Item).ToList();

                if (invoice == null)
                {
                    return NotFound();
                }
                return invoice;
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
        #endregion

        #region GetInvoice
        // GET: api/Invoices/5
        [HttpGet("GetInvoice/{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            try
            {
                var invoice = await _context.Invoices.SingleAsync(inv => inv.InvoiceId == id);

                _context.Entry(invoice)
                    .Collection(inv => inv.InvoiceItems)
                    .Query()
                    .Include(invItem => invItem.Item)
                    .Load();

                if (invoice == null)
                {
                    return NotFound();
                }

                return invoice;
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }

        }

        #endregion

        #region SaveInvoice
        // POST: api/Invoices
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("SaveInvoice")]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            try
            {
                var TotalTax = invoice.InvoiceItems.Sum(x => x.TaxAmount);
                var TotalExcelAmount = invoice.InvoiceItems.Sum(x => x.ExclAmount);
                var TotalInclAmount = invoice.InvoiceItems.Sum(x => x.InclAmount);

                invoice.TotalExcl = TotalExcelAmount;
                invoice.TotalTax = TotalTax;
                invoice.TotalIncl = TotalInclAmount;

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetInvoice", new { id = invoice.InvoiceId }, invoice);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }

        }
        #endregion

        #region EditInvoice
        // PUT: api/Invoices/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("EditInvoice/{id}")]
        public async Task<IActionResult> PutInvoice(int id, Invoice invoice)
        {
            if (id != invoice.InvoiceId)
            {
                return BadRequest();
            }

            try
            {
                var TotalTax = invoice.InvoiceItems.Sum(x => x.TaxAmount);
                var TotalExcelAmount = invoice.InvoiceItems.Sum(x => x.ExclAmount);
                var TotalInclAmount = invoice.InvoiceItems.Sum(x => x.InclAmount);

                invoice.TotalExcl = TotalExcelAmount;
                invoice.TotalTax = TotalTax;
                invoice.TotalIncl = TotalInclAmount;

                _context.Entry(invoice).State = EntityState.Modified;

                var invoiceItems = invoice.InvoiceItems.Where(inv => inv.InvoiceId == id).ToList();
                foreach (var item in invoiceItems)
                {
                    _context.Entry(item).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetInvoice", new { id = invoice.InvoiceId }, invoice);
        }
        #endregion

        #region DeleteInvoice
        // DELETE: api/Invoices/5
        [HttpDelete("DeleteInvoice/{id}")]
        public async Task<ActionResult<Invoice>> DeleteInvoice(int id)
        {
            try
            {
                var invoice = _context.Invoices.Find(id);

                if (invoice == null)
                {
                    return NotFound();
                }

                var invoiceItems = _context.InvoiceItems.Where(inv => inv.InvoiceId == invoice.InvoiceId).ToList();
                foreach (var item in invoiceItems)
                {
                    _context.InvoiceItems.Remove(item);
                }

                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();

                return invoice;
            }
            catch (Exception ex)
            {

                return NotFound(ex);
            }
        }

        #endregion

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.InvoiceId == id);
        }
    }
}

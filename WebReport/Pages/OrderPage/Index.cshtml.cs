using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebReport.Models;

namespace WebReport.Pages.OrderPage
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> Orders { get; set; }

        public async Task OnGet()
        {
            Orders = await _context.Orders.ToListAsync();
        }

        public async Task<IActionResult> OnPostDelete(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var order = await _context.Orders.FindAsync(id.Value);

            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}

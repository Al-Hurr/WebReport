using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebReport.Models;

namespace WebReport.Pages.OrderPage
{
    public class EditModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; }
        public async Task<IActionResult> OnGet (int? id)
        {
            if (!id.HasValue)
                return NotFound();

            Order = await _context.Orders.FindAsync(id.Value);
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Orders.Update(Order);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}

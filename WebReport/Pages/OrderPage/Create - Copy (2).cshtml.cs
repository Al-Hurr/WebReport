using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebReport.Models;

namespace WebReport.Pages.OrderPage
{
    public class CreateModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            await _context.Orders.AddAsync(Order);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}

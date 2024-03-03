using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using WaterDrinkingLogger.Models;

namespace WaterDrinkingLogger.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DrinkingWaterModel DrinkingWater { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                SqlConnection connection = new(_configuration.GetConnectionString("DefaultConnectionString"));
                connection.Open();
                string query = $"INSERT INTO drinking_water(Date, Quantity, Measure) VALUES ('{DrinkingWater.Date}', {DrinkingWater.Quantity}, '{DrinkingWater.Measure}')";
                SqlCommand command = new(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
                
                return RedirectToPage("./Index");
            }
            catch (Exception)
            {
                return RedirectToPage("./Error");
            }

        }
    }
}

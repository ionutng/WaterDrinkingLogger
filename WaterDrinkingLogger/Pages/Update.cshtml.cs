using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using WaterDrinkingLogger.Models;

namespace WaterDrinkingLogger.Pages;

public class UpdateModel : PageModel
{
    private readonly IConfiguration _configuration;

    [BindProperty]
    public DrinkingWaterModel DrinkingWater { get; set; }

    public UpdateModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult OnGet(int id)
    {
        DrinkingWater = GetById(id);

        return Page();
    }

    private DrinkingWaterModel GetById(int id)
    {
        try
        {
            var drinkingWaterRecord = new DrinkingWaterModel();

            SqlConnection connection = new(_configuration.GetConnectionString("DefaultConnectionString"));
            connection.Open();

            string query = $"SELECT * FROM drinking_water WHERE Id = {id}";
            SqlCommand command = new(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                drinkingWaterRecord.Id = reader.GetInt32(0);
                drinkingWaterRecord.Date = DateOnly.FromDateTime(reader.GetDateTime(1));
                drinkingWaterRecord.Quantity = reader.GetInt32(2);
            }

            connection.Close();
            return drinkingWaterRecord;
        }
        catch (Exception)
        {
            return new DrinkingWaterModel();
        }
    }

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

            string query = $"UPDATE drinking_water SET Date = '{DrinkingWater.Date}', Quantity = {DrinkingWater.Quantity} WHERE Id = {DrinkingWater.Id}";
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

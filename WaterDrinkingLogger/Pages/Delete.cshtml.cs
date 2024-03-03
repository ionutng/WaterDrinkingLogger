using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using WaterDrinkingLogger.Models;

namespace WaterDrinkingLogger.Pages;

public class DeleteModel : PageModel
{
    private readonly IConfiguration _configuration;

    [BindProperty]
    public DrinkingWaterModel DrinkingWater { get; set; }

    public DeleteModel(IConfiguration configuration)
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
                drinkingWaterRecord.Quantity = reader.GetDouble(2);
                drinkingWaterRecord.Measure = reader.GetString(3);
            }

            connection.Close();
            return drinkingWaterRecord;
        }
        catch (Exception)
        {
            return new DrinkingWaterModel();
        }
    }

    public IActionResult OnPost(int id)
    {
        SqlConnection connection = new(_configuration.GetConnectionString("DefaultConnectionString"));
        connection.Open();
        string query = $"DELETE FROM drinking_water WHERE Id = {id}";
        SqlCommand command = new(query, connection);
        command.ExecuteNonQuery();

        return RedirectToPage("./Index");
    }
}

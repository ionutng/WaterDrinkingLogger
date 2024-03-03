using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using WaterDrinkingLogger.Models;

namespace WaterDrinkingLogger.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<DrinkingWaterModel> Records { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            Records = GetAllRecords();
            ViewData["Total"] = Records.AsEnumerable().Sum(x => x.Quantity * GetMeasureAmount(x.Measure));
        }

        private List<DrinkingWaterModel> GetAllRecords()
        {
            try
            {
                SqlConnection connection = new(_configuration.GetConnectionString("DefaultConnectionString"));
                connection.Open();
                string query = $"SELECT * FROM drinking_water";
                SqlCommand command = new(query, connection);

                var tableData = new List<DrinkingWaterModel>();
                SqlDataReader reader = command.ExecuteReader();
            
                while (reader.Read())
                {
                    tableData.Add(
                        new DrinkingWaterModel
                        {
                            Id = reader.GetInt32(0),
                            Date = DateOnly.FromDateTime(reader.GetDateTime(1)),
                            Quantity = reader.GetDouble(2),
                            Measure = reader.GetString(3),
                        });
                }
            
                connection.Close();
            
                return tableData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

        private double GetMeasureAmount(string measure)
        {
            if (measure.Trim() == "Glass (250ml)")
                return 0.250;
            else if (measure.Trim() == "Bottle (1l)")
                return 1;
            else if (measure.Trim() == "Big Bottle (2l)")
                return 2;
            else
                return 0;
        }
    }
}

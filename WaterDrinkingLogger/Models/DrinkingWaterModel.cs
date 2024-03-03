using System.ComponentModel.DataAnnotations;
using WaterDrinkingLogger.Validation;

namespace WaterDrinkingLogger.Models;

public class DrinkingWaterModel
{
    public int Id { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}", ApplyFormatInEditMode = true)]
    [DateLessThanOrEqualToToday]
    public DateOnly Date { get; set; }

    [Range(0, Int32.MaxValue, ErrorMessage = "Value for {0} must be positive.")]
    public int Quantity { get; set; }
}

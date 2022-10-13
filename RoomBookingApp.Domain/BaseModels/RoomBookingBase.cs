using System.ComponentModel.DataAnnotations;

namespace RoomBookingApp.Core.Models;

public abstract class RoomBookingBase
{
    [Required]
    public string FullName { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public DateTime Date { get; set; }
}

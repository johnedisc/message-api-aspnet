using System.ComponentModel.DataAnnotations;

namespace MessageBoardApi.Models
{
  public class Message
  {
    public int MessageId { get; set; }
    [Required]
    [StringLength(20)]
    public string Name { get; set; }
    [Required]
    [StringLength(200, ErrorMessage = "Message must be between 0 && 200")]
    public string MessageString { get; set; }
    // [Range(0, 200, ErrorMessage = "Date must be between 0 && 200")]
    // public DateTime Date { get; set; } 
    // public DateTime Date = DateTime.Now;


  }
}
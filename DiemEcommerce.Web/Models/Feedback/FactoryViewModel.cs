using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DiemEcommerce.Web.Models.Feedback;

public class FeedbackViewModel
{
    public Guid Id { get; set; }
    public Guid OrderDetailId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductImage { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public List<FeedbackMediaViewModel> Images { get; set; } = new List<FeedbackMediaViewModel>();
    public DateTimeOffset CreatedOnUtc { get; set; }
}

public class FeedbackMediaViewModel
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
}

public class CreateFeedbackRequestModel
{
    [Required]
    public Guid OrderDetailId { get; set; }
    
    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }
    
    [Required]
    public string Comment { get; set; } = string.Empty;
    
    public List<IFormFile>? Images { get; set; }
}

public class UpdateFeedbackRequestModel
{
    [Required]
    public Guid FeedbackId { get; set; }
    
    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }
    
    [Required]
    public string Comment { get; set; } = string.Empty;
    
    public List<IFormFile>? NewImages { get; set; }
    
    public List<Guid>? DeleteImages { get; set; }
}
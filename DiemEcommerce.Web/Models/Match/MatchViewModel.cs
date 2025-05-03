using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Models.Feedback;

namespace DiemEcommerce.Web.Models.Match;

public class MatchViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IEnumerable<MatchMediaViewModel> CoverImages { get; set; } = Array.Empty<MatchMediaViewModel>();
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public Guid FactoryId { get; set; }
    public string FactoryName { get; set; } = string.Empty;
    public string FactoryAddress { get; set; } = string.Empty;
    public string FactoryPhoneNumber { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class MatchDetailViewModel : MatchViewModel
{
    public string FactoryEmail { get; set; } = string.Empty;
    public string FactoryWebsite { get; set; } = string.Empty;
    public string FactoryDescription { get; set; } = string.Empty;
    public string FactoryTaxCode { get; set; } = string.Empty;
    public string FactoryBankAccount { get; set; } = string.Empty;
    public string FactoryBankName { get; set; } = string.Empty;
    public string FactoryLogo { get; set; } = string.Empty;
    public ICollection<FeedbackViewModel> Feedbacks { get; set; } = Array.Empty<FeedbackViewModel>();
}

public class MatchMediaViewModel
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
}
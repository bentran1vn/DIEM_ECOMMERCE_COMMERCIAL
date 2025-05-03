namespace DiemEcommerce.Web.Models.Factory;

public class FactoryViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public string TaxCode { get; set; } = string.Empty;
    public string BankAccount { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
}

// FactoryDetailViewModel can inherit from FactoryViewModel or have additional properties if needed
public class FactoryDetailViewModel : FactoryViewModel
{
    // Add any additional properties specific to the detail view
}
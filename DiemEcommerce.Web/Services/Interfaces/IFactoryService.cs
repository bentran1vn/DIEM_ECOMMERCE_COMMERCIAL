using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Models.Factory;

namespace DiemEcommerce.Web.Services.Interfaces;

public interface IFactoryService
{
    Task<PaginatedList<FactoryViewModel>?> GetAllFactoriesAsync(int pageIndex = 1, int pageSize = 10, string? searchTerm = null);
    Task<FactoryDetailViewModel?> GetFactoryByIdAsync(Guid id);
}
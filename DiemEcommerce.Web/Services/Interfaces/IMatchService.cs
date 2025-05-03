using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Models.Match;

namespace DiemEcommerce.Web.Services.Interfaces;

public interface IMatchService
{
    Task<PaginatedList<MatchViewModel>?> GetAllMatchesAsync(int pageIndex = 1, int pageSize = 10, string? searchTerm = null, List<Guid>? categoryIds = null);
    Task<MatchDetailViewModel?> GetMatchByIdAsync(Guid id);
    Task<PaginatedList<MatchViewModel>?> GetMatchesByFactoryIdAsync(Guid factoryId);
}
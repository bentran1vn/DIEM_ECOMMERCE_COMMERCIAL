using DiemEcommerce.Web.Models.Common;
using DiemEcommerce.Web.Models.Feedback;

namespace DiemEcommerce.Web.Services.Interfaces;

public interface IFeedbackService
{
    Task<PaginatedList<FeedbackViewModel>?> GetFeedbackByProductAsync(Guid matchId, int pageIndex = 1, int pageSize = 10);
    Task<PaginatedList<FeedbackViewModel>?> GetFeedbackByCustomerAsync(int pageIndex = 1, int pageSize = 10);
    Task<FeedbackViewModel?> GetFeedbackByIdAsync(Guid feedbackId);
    Task<FeedbackViewModel?> CreateFeedbackAsync(CreateFeedbackRequestModel model);
    Task<FeedbackViewModel?> UpdateFeedbackAsync(UpdateFeedbackRequestModel model);
    Task<bool> DeleteFeedbackAsync(Guid feedbackId);
}
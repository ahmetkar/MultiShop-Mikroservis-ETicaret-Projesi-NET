namespace MultiShop.WebUI.Services.StatisticServices.CommentStatisticServices
{
    public interface ICommentStasticService
    {
        Task<int> GetActiveCommentCount();
        Task<int> GetTotalCommentCount();
    }
}

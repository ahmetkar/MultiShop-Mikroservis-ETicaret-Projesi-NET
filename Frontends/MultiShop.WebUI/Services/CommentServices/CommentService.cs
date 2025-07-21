using MultiShop.DtoLayer.CommentDtos;

namespace MultiShop.WebUI.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly HttpClient _httpClient;

        public CommentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateCommentAsync(CreateCommentDto createCommentDto)
        {
            await _httpClient.PostAsJsonAsync<CreateCommentDto>("Comments", createCommentDto);

        }

        public async Task DeleteCommentAsync(string id)
        {
            await _httpClient.DeleteAsync("Comments?id=" + id);
        }

        public async Task<List<ResultCommentDto>> GetAllCommentAsync()
        {
            var resp = await _httpClient.GetAsync("Comments/CommentList");
            var values = await resp.Content.ReadFromJsonAsync<List<ResultCommentDto>>();
            return values;
        }

        public async Task<UpdateCommentDto> GetByIdComment(string id)
        {
            var resp = await _httpClient.GetAsync("Comments/" + id);
            var values = await resp.Content.ReadFromJsonAsync<UpdateCommentDto>();
            return values;

        }

        public async Task<List<ResultCommentDto>> GetCommentsByProductId(string id)
        {
            var resp = await _httpClient.GetAsync($"Comments/CommentsByProductId/{id}");
            var values = await resp.Content.ReadFromJsonAsync<List<ResultCommentDto>>();
            return values;
        }

        public async Task UpdateCommentAsync(UpdateCommentDto updateCommentDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateCommentDto>("Comments", updateCommentDto);

        }
    }
}

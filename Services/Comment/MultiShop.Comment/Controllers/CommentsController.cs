using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Comment.Context;
using MultiShop.Comment.DTOs;
using MultiShop.Comment.Entities;

namespace MultiShop.Comment.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CommentContext _context;
        private readonly IMapper _mapper;

        public CommentsController(CommentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var comments = _context.UserComments.ToList();
            var values = _mapper.Map<List<ResultCommentDto>>(comments);
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateComment(CreateCommentDto comment)
        {
            var values = _mapper.Map<UserComment>(comment);
            _context.UserComments.Add(values);
            _context.SaveChanges();
            return Ok("Yorum Eklendi.");
        }

        [HttpPut]
        public IActionResult UpdateComment(UpdateCommentDto comment)
        {
            var values = _mapper.Map<UserComment>(comment);
            _context.UserComments.Update(values);
            _context.SaveChanges();
            return Ok("Yorum başarıyla güncellendi.");
        }

        [HttpDelete]
        public IActionResult DeleteComment(int id)
        {
            var comment = _context.UserComments.Find(id);
            _context.UserComments.Remove(comment);
            _context.SaveChanges();
            return Ok("Yorum başarıyla silindi.");
        }

        [HttpGet("{id}")]
        public IActionResult GetCommentById(int id)
        {
            var comment = _context.UserComments.Find(id);
            if (comment == null)
            {
                return NotFound("Yorum bulunamadı.");
            }
            var values = _mapper.Map<ResultCommentDto>(comment);
            return Ok(values);

        }

        [HttpGet("GetCommentsByProductId/{id}")]
        public IActionResult GetCommentsByProductId(string id)
        {
            var comments = _context.UserComments.Where(x => x.ProductId == id).ToList();
            var values = _mapper.Map<List<ResultCommentDto>>(comments);
            return Ok(values);
        }


        [HttpGet("GetActiveCommentCount")]
        public IActionResult GetActiveCommentCount()
        {
            int value = _context.UserComments.Where(x => x.Status == true).Count();
            return Ok(value);
        }

        [HttpGet("GetTotalCommentCount")]
        public IActionResult GetTotalCommentCount()
        {
            int value = _context.UserComments.Count();
            return Ok(value);
        }


    }   
}

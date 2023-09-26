using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Interfaces;
using PokemonReview.Models;
using PokemonReview.Repository;

namespace PokemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var review = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int id)
        {
            if(!_reviewerRepository.ReviwerExist(id))
                return BadRequest();
            var review = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }

        [HttpGet("review/{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviwerExist(reviewerId))
                return BadRequest();

            var review = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewByReviewer(reviewerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }



        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);

            var pokemon = _reviewerRepository.GetReviewers().Where(x => x.LastName.Trim().ToUpper()
            == reviewerCreate.LastName.Trim().ToUpper()).FirstOrDefault();

            if (pokemon != null)
            {
                ModelState.AddModelError("", "Reviewer already exist");

            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

            if (!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Error while adding new reviewer..");
                return StatusCode(400, ModelState);
            }
            return Ok("Succefully Created.");

        }


        [HttpPut("{reviewerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Updatereviewer(int reviewerId, [FromBody] ReviewerDto reviewer)
        {
            if (reviewer == null)
                return BadRequest(ModelState);

            if (reviewerId != reviewer.Id)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviwerExist(reviewerId))
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewer);

            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Error updating the reviewer");
                return StatusCode(400, ModelState);
            }
            return NoContent();
        }


        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCategory([FromQuery] int reviewerId)
        {
            if (!_reviewerRepository.ReviwerExist(reviewerId))
                return BadRequest(ModelState);

            var categoryToDelete = _reviewerRepository.GetReviewer(reviewerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.DeleteReviewer(categoryToDelete))
            {
                ModelState.AddModelError("", "Error delating the reviewer");
                return StatusCode(400, ModelState);
            }
            return NoContent();
        }




    }
}

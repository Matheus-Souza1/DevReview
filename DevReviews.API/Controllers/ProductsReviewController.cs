using DevReviews.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevReviews.API.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/productreviews")]
    public class ProductsReviewController : ControllerBase
    {
        //GET api/products/1/productreviews/5
        [HttpGet("{id}")]
        public IActionResult GetById(int productId, int id)
        {
            return Ok();
        }

        //POST api/products/1/productreviews
        [HttpPost]
        public IActionResult Post(int productId, AddProductReviewInputModel model)
        {
            return CreatedAtAction(nameof(GetById), new { id = 1, productId = 2 }, model);
        }
    }
}
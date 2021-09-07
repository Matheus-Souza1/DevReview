using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DevReviews.API.Entities;
using DevReviews.API.Models;
using DevReviews.API.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DevReviews.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly DevReviewsDbContext _dbContext;
        private readonly IMapper _mapper;
        public ProductsController(DevReviewsDbContext dbcontext, IMapper mapper)
        {
            _dbContext = dbcontext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _dbContext.Products;

            var productsViewModel = _mapper.Map<List<ProductViewModel>>(products);

            return Ok(productsViewModel);
        }

        //GET api/products/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _dbContext.Products.SingleOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productDetails = _mapper.Map<ProductDetailsViewModel>(product);
            
            return Ok(productDetails);
        }

        //POST api/products/
        [HttpPost]
        public IActionResult Post(AddProductInputModel model)
        {
            var product = new Product(model.Title, model.Description, model.Price);

            _dbContext.Products.Add(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, model);
        }

        //PUT api/products/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateProductInputModel model)
        {
            if (model.Description.Length > 50)
            {
                return BadRequest();
            }

            var product = _dbContext.Products.SingleOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            product.Update(model.Description, model.Price);

            return NoContent();
        }
    }
}
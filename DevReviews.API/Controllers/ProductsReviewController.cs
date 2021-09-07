using System.Threading.Tasks;
using AutoMapper;
using DevReviews.API.Entities;
using DevReviews.API.Models;
using DevReviews.API.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevReviews.API.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/productreviews")]
    public class ProductsReviewController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductsReviewController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/products/1/productreviews/5
        /// <summary>
        /// Retorna o review de um produto
        /// </summary>
        /// <param name="productId">Identificador do produto</param>
        /// <param name="id">identificador do review</param>
        /// <returns>Objeto com review do produto</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int productId, int id)
        {
            var productReview = await _repository.GetReviewByIdAsync(id);

            if (productReview == null)
            {
                return NotFound();
            }

            var productDetails = _mapper.Map<ProductReviewDetailViewModel>(productReview);

            return Ok(productDetails);
        }

        //POST api/products/1/productreviews
        /// <summary>
        /// Cadastro de review de um produto
        /// </summary>
        /// <remarks>Requisição:
        /// {
        ///  "rating": "10,
        ///  "author": "Matheus Souza",
        ///  "comments": "Melhor Havaina que ja usei"
        /// }
        /// </remarks>
        /// <param name="productId">Identificador do produto</param>
        /// <param name="model">Objeto com dados de review</param>
        /// <returns>Obejto recem-criado</returns>
        /// <response code="201">Sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(int productId, AddProductReviewInputModel model)
        {
            var productReview = new ProductReview(model.Author, model.Rating, model.Comments, productId);

            await _repository.AddReviewAsync(productReview);

            return CreatedAtAction(nameof(GetById), new { id = productReview.Id, productId = productId }, model);
        }
    }
}
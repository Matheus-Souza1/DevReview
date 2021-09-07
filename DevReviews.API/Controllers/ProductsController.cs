using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevReviews.API.Entities;
using DevReviews.API.Models;
using DevReviews.API.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DevReviews.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna todos os dados do produto
        /// </summary>
        /// <returns>Objeto de detalhes do produto</returns>
        /// <response code="404">Produtos não encontrado</response>
        /// <response code="200">Produtos encontrado</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Log.Information("Metodo GET chamado");

            var products = await _repository.GetAllAsync();

            var productsViewModel = _mapper.Map<List<ProductViewModel>>(products);

            return Ok(productsViewModel);
        }

        //GET api/products/{id}
        /// <summary>
        /// Retorna um Produto especifico
        /// </summary>
        /// <param name="id">Identificador do produto</param>
        /// <returns>Objeto com dados do produto</returns>
        /// <response code="404">Produto não encontrado</response>
        /// <response code="200">Produto encontrado</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _repository.GetDetailsByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var productDetails = _mapper.Map<ProductDetailsViewModel>(product);

            return Ok(productDetails);
        }

        //POST api/products/
        /// <summary>Cadastro de Produto</summary>
        /// <remarks>Requisição:
        /// {
        ///  "title": "Chinelo",
        ///  "description": "Um chinelo da marca Havaiana",
        ///  "price": 50
        /// }
        /// </remarks>
        /// <param name="model">Objeto com dados de cadastro de Produto</param>
        /// <returns>Objeto recém-criado</returns>
        /// <response code="201">Objeto criado com Sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(AddProductInputModel model)
        {
            var product = new Product(model.Title, model.Description, model.Price);

            await _repository.AddAsync(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, model);
        }

        //PUT api/products/{id}
        /// <summary>
        /// Atualização do produto
        /// </summary>        /// 
        /// <remarks>Requisição:
        /// {
        ///  "description": "Melhor Havaina de todas",
        ///  "price": 50
        /// }
        /// </remarks>
        /// <param name="id">Identificador do produto</param>
        /// <param name="model">Objeto com dados do produto</param>
        /// <returns>Objeto Atualizado</returns>
        /// <response code="201">Objeto atualizado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, UpdateProductInputModel model)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Update(model.Description, model.Price);
            await _repository.UpdateAsync(product);

            return NoContent();
        }
    }
}
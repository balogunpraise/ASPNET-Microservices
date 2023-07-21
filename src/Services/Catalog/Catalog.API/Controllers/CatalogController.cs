using Catalog.API.Entitites;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private readonly ILogger<CatalogController> _logger;
        public CatalogController(IProductRepository repo, ILogger<CatalogController> logger)
        {
            _repo = repo ?? throw new ArgumentException(nameof(repo));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        #region Query

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> Products()
        {
            var products = await _repo.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _repo.GetProductById(id);
            if(product is null)
            {
                _logger.LogError($"Product with id: {id}, not found");
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet]
        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductByCategory(string category)
        {
            var products = await _repo.GetProductByCategory(category);
            return Ok(products);
        }

        #endregion

        #region Command
        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> Product([FromBody] Product product)
        {
            await _repo.CreateProduct(product);
            return CreatedAtRoute("GetProdunct", new {id = product.Id}, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] Product product)
        {
            return Ok(await _repo.UpdateProduct(product));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _repo.DeleteProduct(id));
        }

        #endregion
    }
}

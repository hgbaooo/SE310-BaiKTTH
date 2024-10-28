using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SE310.P12_BaiKTTH_BE.Dto;
using SE310.P12_BaiKTTH_BE.Interfaces;
using SE310.P12_BaiKTTH_BE.Models;

namespace SE310.P12_BaiKTTH_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        
        [HttpGet("getAllProducts")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetProductDto>))]
        public IActionResult GetCategories()
        {
            var products = _mapper.Map<List<GetProductDto>>(_productRepository.GetProducts());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(products);
        }
        
        [HttpGet("getProductById/{Id}")]
        [ProducesResponseType(200, Type = typeof(GetProductDto))]
        [ProducesResponseType(400)]
        public IActionResult GetProductById(int Id)
        {
            var product = _mapper.Map<List<GetProductDto>>(
                _productRepository.GetProductById(Id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(product);
        }
        
        [HttpGet("getProductByCategoryId/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<GetProductDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetProductsByCategoryId(int categoryId)
        {
            // Check if the category exists
            if (!_productRepository.CategoryExist(categoryId))
            {
                return NotFound($"Category with ID {categoryId} does not exist.");
            }

            var products = _mapper.Map<List<ProductDto>>(
                _productRepository.GetProductsByCategoryId(categoryId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(products);
        }

        
        [HttpPost("createProduct")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] ProductDto productCreate)
        {
            if (productCreate == null)
                return BadRequest(ModelState);

            // Ensure that CategoryId is provided
            if (productCreate.CategoryId == 0)
            {
                ModelState.AddModelError("", "CategoryId is required");
                return BadRequest(ModelState);
            }

            // Validate if the provided CategoryId exists
            var categoryExists = _productRepository.CategoryExist(productCreate.CategoryId);
            if (!categoryExists)
            {
                ModelState.AddModelError("", "Category does not exist");
                return BadRequest(ModelState);
            }

            // Check if product already exists
            var product = _productRepository.GetProducts()
                .Where(c => c.Name.Trim().ToUpper() == productCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if(product != null)
            {
                ModelState.AddModelError("", "Product already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productMap = _mapper.Map<Product>(productCreate);

            if(!_productRepository.CreateProduct(productMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        
        [HttpPatch("updateProduct/{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(int productId, [FromBody]ProductDto updatedProduct)
        {
            if (updatedProduct == null)
                return BadRequest(ModelState);
            
            if (!_productRepository.ProductExist(productId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var productMap = _mapper.Map<Product>(updatedProduct);

            if(!_productRepository.UpdateProduct(productMap))
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }
        
        [HttpDelete("deleteProduct/{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int productId)
        {
            if(!_productRepository.ProductExist(productId))
            {
                return NotFound();
            }
            
            if(!_productRepository.DeleteProduct(productId))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return Ok("Successfully deleted");
        }
    }
}
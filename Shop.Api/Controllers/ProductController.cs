using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.DTOs.ProductDTOs;
using Shop.Core.Entities;
using Shop.Core.Repositories;
using Shop.Data.FileManager;

namespace Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _env;

        public ProductController(IProductRepository productRepository, IWebHostEnvironment env)
        {
            _productRepository = productRepository;
            _env = env;
        }
        [HttpPost("")]
        [Consumes("multipart/form-data")]

        public ActionResult<ProductPostDTO> Create([FromForm] ProductPostDTO postDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Product product = new Product()
            {
                SalePrice = postDTO.SalePrice,
                CostPrice = postDTO.CostPrice,
                DiscountPercent = postDTO.DiscountPercent,
                BrandId = postDTO.BrandId,
                Name = postDTO.Name,
                Image = FileManager.SaveImage(_env.WebRootPath, "uploads/ProductImage", postDTO.ProductImage)
            };
            _productRepository.Add(product);
            _productRepository.Commit();

            return StatusCode(201,product.Id);
        }
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public ActionResult<ProductPutDTO> Edit(int id,ProductPutDTO productPutDTO)
        {
            Product product=_productRepository.Get(x=>x.Id==id);
            if (product==null)
            {
                return StatusCode(404);
            }
            string oldImageName = null;
            if (productPutDTO.ProductImage!=null)
            {
                oldImageName = product.Image;
                product.Image = FileManager.SaveImage(_env.WebRootPath, "uploads/ProductImage", productPutDTO.ProductImage);

            }
           

            product.Name = productPutDTO.Name;
            product.CostPrice = productPutDTO.CostPrice;
            product.SalePrice = productPutDTO.SalePrice;
            product.BrandId=productPutDTO.BrandId;
            product.DiscountPercent=productPutDTO.DiscountPercent;

            _productRepository.Commit();
            if(oldImageName!=null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/ProductImage", oldImageName);
            }

            return StatusCode(200);
            
           
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           Product product=_productRepository.Get(x=>x.Id == id);
            if (product==null)
            {
                return StatusCode(404);
            }
            string oldImageName = null;
           if(product.Image!=null)
            {
                oldImageName=product.Image;
            }
            _productRepository.Remove(product);
            if(oldImageName!=null) 
            {
                FileManager.Delete(_env.WebRootPath, "uploads/ProductImage", oldImageName);
            }
            _productRepository.Commit();

            return StatusCode(201);
        }

        [HttpGet("")]

        public ActionResult GetAllItem()
        {
            var data = _productRepository.GetAll("Brand").ToList();
            return StatusCode(200, data);
        }

    }
}

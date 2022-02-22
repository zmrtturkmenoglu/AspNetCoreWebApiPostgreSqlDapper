using AspNetCoreWebApiPostgreSqlDapper.Model;
using AspNetCoreWebApiPostgreSqlDapper.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApiPostgreSqlDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : CustomBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService ProductService)
        {
            _productService = ProductService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _productService.GetAll());
        }

        //api/products/4
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CreateActionResultInstance(await _productService.GetById(id));
        }

        //api/products/getbycode/4
        [HttpGet]
        [Route("/api/[controller]/[action]/{code}")]
        public async Task<IActionResult> GetByCode(string categoryId)
        {
            return CreateActionResultInstance(await _productService.GetByCategoryId(categoryId));
        }

        [HttpPost]
        public async Task<IActionResult> Save(Product Product)
        {
            return CreateActionResultInstance(await _productService.Save(Product));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Product Product)
        {
            return CreateActionResultInstance(await _productService.Update(Product));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResultInstance(await _productService.Delete(id));
        }
    }

    public class CustomBaseController : ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}

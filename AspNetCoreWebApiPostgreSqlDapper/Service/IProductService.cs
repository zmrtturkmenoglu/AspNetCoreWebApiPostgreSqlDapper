using AspNetCoreWebApiPostgreSqlDapper.Model;

namespace AspNetCoreWebApiPostgreSqlDapper.Service
{
    public interface IProductService
    {
        Task<Response<List<Product>>> GetAll();
        Task<Response<Product>> GetById(int id);
        Task<Response<NoContent>> Save(Product product);
        Task<Response<NoContent>> Update(Product product);
        Task<Response<NoContent>> Delete(int id);
        Task<Response<Product>> GetByCategoryId(string categoryId);
    }
}

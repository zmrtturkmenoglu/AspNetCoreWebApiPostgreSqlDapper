using AspNetCoreWebApiPostgreSqlDapper.Model;
using Dapper;
using Npgsql;
using System.Data;

namespace AspNetCoreWebApiPostgreSqlDapper.Service
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var statusDelete = await _dbConnection.ExecuteAsync("delete from product where id=@Id", new { Id = id });
            return statusDelete > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Product not found!", 404);
        }

        public async Task<Response<List<Product>>> GetAll()
        {
            var products = await _dbConnection.QueryAsync<Product>("select * from product");
            return Response<List<Product>>.Success(products.ToList(), 200);
        }

        public async Task<Response<Product>> GetByCategoryId(string categoryId)
        {
            var product = (await _dbConnection.QueryAsync<Product>("select * from product where categoryid=@CategoryId",
                 new { CategoryId = categoryId })).FirstOrDefault();

            if (product == null)
            {
                return Response<Product>.Fail("Product not found!", 404);
            }
            return Response<Product>.Success(product, 200);
        }

        public async Task<Response<Product>> GetById(int id)
        {
            var product = (await _dbConnection.QueryAsync<Product>("select * from product where id=@Id", new { Id = id })).SingleOrDefault();
            if (product == null)
            {
                return Response<Product>.Fail("Product not found!", 404);
            }
            return Response<Product>.Success(product, 200);
        }

        public async Task<Response<NoContent>> Save(Product product)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("INSERT INTO product(categoryid, name, description, price) VALUES (@CategoryId, @Name, @Description, @Price)", product);
            if (saveStatus > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("an error occured while adding", 500);
        }

        public async Task<Response<NoContent>> Update(Product product)
        {
            var updateStatus = await _dbConnection.ExecuteAsync("update product set categoryid=@CategoryId, name=@Name, description=@Description, price=@Price where id=@Id",
               new { Id = product.Id, CategoryId = product.CategoryId, Name = product.Name, Description = product.Description, Price = product.Price });

            if (updateStatus > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("Product not found!", 404);
        }
    }
}

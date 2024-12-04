using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdoSqlWebapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        [HttpGet(Name = "GetInt")]
        public int Get()
        {
            return 100;
        }
        /*[HttpGet]
        public IActionResult GetData()
        {
            try
            {
                // Load data from the JSON file
                List<DataObject> data = LoadDataFromJson();

                // Return the data
                return Ok(data);
            }
            catch (Exception ex)
            {
                // Handle file read errors or other exceptions
                return StatusCode(500, $"An error occurred while retrieving data: {ex.Message}");
            }
        }*/
        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            try
            { // Get connection string from configuration
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("GetProducts", myConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        await myConnection.OpenAsync();
                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            List<ProductModel> products = new List<ProductModel>();
                            while (await reader.ReadAsync())
                            {
                                products.Add(new ProductModel { ProductId = reader.GetInt32(0), ProductName = reader.GetString(1), ProductType = reader.GetString(2), ProductShopName = reader.GetString(3), ProductLocation = reader.GetString(4) });
                            }
                            return Ok(products);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log exception (consider using a logging framework)
                return StatusCode(500, $"A database error occurred. {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         // } }

        [HttpPost]
        public async Task<IActionResult> PostData([FromBody] ProductModel data)
        {
            if (data == null)
            {
                return BadRequest("Invalid input data.");
            }

            try
            {
                // Get connection string from configuration
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    string query = @"
                        INSERT INTO tblProduct (ProductName,ProductType,ProductShopName,ProductLocation)
                        VALUES (@ProductName,@ProductType,@ProductShopName,@ProductLocation)";

                    using (SqlCommand sqlCmd = new SqlCommand(query, myConnection))
                    {
                        sqlCmd.CommandType = CommandType.Text;

                        // Add parameters                        
                        sqlCmd.Parameters.AddWithValue("@ProductName", data.ProductName ?? (object)DBNull.Value);
                        sqlCmd.Parameters.AddWithValue("@ProductType", data.ProductType ?? (object)DBNull.Value);
                        sqlCmd.Parameters.AddWithValue("@ProductShopName", data.ProductShopName ?? (object)DBNull.Value);
                        sqlCmd.Parameters.AddWithValue("@ProductLocation", data.ProductLocation ?? (object)DBNull.Value);

                        await myConnection.OpenAsync();
                        int rowsInserted = await sqlCmd.ExecuteNonQueryAsync();

                        if (rowsInserted > 0)
                        {
                            return Ok("Data saved successfully.");
                        }
                        else
                        {
                            return StatusCode(500, "Failed to insert data into the database.");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log exception (consider using a logging framework)
                return StatusCode(500, $"A database error occurred.{sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
        [HttpPost("InsertProduct")]
        public async Task<IActionResult> InsertProduct([FromBody] ProductModel data)
        {
            if (data == null)
            {
                return BadRequest("Invalid input data.");
            }
            try
            {
                // Get connection string from configuration
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("InsertProduct", myConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        sqlCmd.Parameters.AddWithValue("@ProductName", data.ProductName ?? (object)DBNull.Value);
                        sqlCmd.Parameters.AddWithValue("@ProductType", data.ProductType ?? (object)DBNull.Value);
                        sqlCmd.Parameters.AddWithValue("@ProductShopName", data.ProductShopName ?? (object)DBNull.Value);
                        sqlCmd.Parameters.AddWithValue("@ProductLocation", data.ProductLocation ?? (object)DBNull.Value);

                        await myConnection.OpenAsync();
                        int rowsInserted = await sqlCmd.ExecuteNonQueryAsync();

                        if (rowsInserted > 0)
                        {
                            return Ok("Data saved successfully.");
                        }
                        else
                        {
                            return StatusCode(500, "Failed to insert data into the database.");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log exception (consider using a logging framework)
                return StatusCode(500, $"A database error occurred. {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductModel data)
        {
            if (data == null || data.ProductId <= 0) { return BadRequest("Invalid input data."); }
            try
            { // Get connection string from configuration
                string connectionString = _configuration.GetConnectionString("DefaultConnection"); 
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("UpdateProduct", myConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        // Add parameters
                        sqlCmd.Parameters.AddWithValue("@ProductId", data.ProductId); 
                        sqlCmd.Parameters.AddWithValue("@ProductName", data.ProductName ?? (object)DBNull.Value); 
                        sqlCmd.Parameters.AddWithValue("@ProductType", data.ProductType ?? (object)DBNull.Value); 
                        sqlCmd.Parameters.AddWithValue("@ProductShopName", data.ProductShopName ?? (object)DBNull.Value); 
                        sqlCmd.Parameters.AddWithValue("@ProductLocation", data.ProductLocation ?? (object)DBNull.Value); 
                        await myConnection.OpenAsync(); 
                        int rowsUpdated = await sqlCmd.ExecuteNonQueryAsync();
                        if (rowsUpdated > 0)
                        {
                            return Ok("Product updated successfully.");
                        }
                        else { return NotFound("Product not found."); }
                    }
                }
            }
            catch (SqlException sqlEx)
            { // Log exception (consider using a logging framework)
                return StatusCode(500, $"A database error occurred. {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id <= 0) { return BadRequest("Invalid input data."); }
            try
            { // Get connection string from configuration
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection myConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("DeleteProduct", myConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        // Add parameters
                        sqlCmd.Parameters.AddWithValue("@ProductId", id);
                        await myConnection.OpenAsync();
                        int rowsDeleted = await sqlCmd.ExecuteNonQueryAsync();
                        if (rowsDeleted > 0)
                        {
                            return Ok("Product deleted successfully.");
                        }
                        else
                        {
                            return NotFound("Product not found.");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            { 
                // Log exception (consider using a logging framework)
                return StatusCode(500, $"A database error occurred. {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }

    public class ProductModel

    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductType { get; set; }
        public string? ProductShopName { get; set; }
        public string? ProductLocation { get; set; }
    }
}

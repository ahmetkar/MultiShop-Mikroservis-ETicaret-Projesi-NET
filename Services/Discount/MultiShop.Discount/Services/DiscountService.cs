using Dapper;
using MultiShop.Discount.Context;
using MultiShop.Discount.DTOs;

namespace MultiShop.Discount.Services
{
   
    public class DiscountService : IDiscountService
    {
        private readonly DapperContext _context;
        public DiscountService(DapperContext context)
        {
            _context = context;
        }
        public async Task CreateCouponAsync(CreateDiscountCouponDto createCouponDto)
        {
            string query = "insert into Coupons(Code,Rate,IsActive,ValidDate,ProductId) values (@code,@rate,@isActive,@validDate,@productId)";
            var parameters = new DynamicParameters();
            parameters.Add("@code",createCouponDto.Code);
            parameters.Add("@rate",createCouponDto.Rate);
            parameters.Add("@isActive", createCouponDto.IsActive);
            parameters.Add("@validDate",createCouponDto.ValidDate);
            parameters.Add("@productId", createCouponDto.ProductId);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query,parameters);
            }
           
        }   

        public async Task DeleteCouponAsync(int id)
        {
            string query = "Delete from Coupons where CouponId=@couponId";
            var parameters = new DynamicParameters();
            parameters.Add("@couponId", id);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }

        }

        public async Task<List<ResultDiscountCouponDto>> GetAllCouponAsync()
        {
            string query = "Select * from Coupons";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultDiscountCouponDto>(query);
                return values.ToList();
            }
        }

        public async Task<GetByIdDiscountCouponDto> GetByIdCouponAsync(int id)
        {
            string query = "Select * from Coupons Where CouponId=@couponId";
            var parameters = new DynamicParameters();
            parameters.Add("@couponId", id);

            using (var connection = _context.CreateConnection())
            {
                var value = await connection.QueryFirstOrDefaultAsync<GetByIdDiscountCouponDto>(query, parameters);
                return value;
            }
        
        }

        public async Task<ResultDiscountCouponDto> GetCodeDetailByCode(string code)
        {
            string query = "Select * from Coupons where Code=@code";
            var parameters = new DynamicParameters();
            parameters.Add("@code", code);
            using (var connection = _context.CreateConnection())
            {
                var value = await connection.QueryFirstOrDefaultAsync<ResultDiscountCouponDto>(query, parameters);
                return value;
            }

        }

        public async Task<int> GetDiscountCouponCount()
        {
            string query = "Select Count(*) from Coupons";
       
            using (var connection = _context.CreateConnection())
            {
                var value = await connection.QueryFirstOrDefaultAsync<int>(query);
                return value;
            }   
        }

        public int GetDiscountCouponCountRate(string code)
        {
            string query = "Select Rate from Coupons where Code=@code";
            var parameters = new DynamicParameters();
            parameters.Add("@code", code);
            using (var connection = _context.CreateConnection())
            {
                var value =connection.QueryFirstOrDefault<ResultDiscountCouponDto>(query, parameters);
                return value.Rate;
            }

        }

        public async Task UpdateCouponAsync(UpdateDiscountCouponDto updateCouponDto)
        {
            string query = "UPDATE Coupons SET Code=@code,Rate=@rate,IsActive=@isActive,ValidDate=@validDate,ProductId=@productId where CouponId=@couponId";
            var parameters = new DynamicParameters();
            parameters.Add("@couponId", updateCouponDto.CouponId);
            parameters.Add("@code", updateCouponDto.Code);
            parameters.Add("@rate", updateCouponDto.Rate);
            parameters.Add("@isActive", updateCouponDto.IsActive);
            parameters.Add("@validDate", updateCouponDto.ValidDate);
            parameters.Add("@productId", updateCouponDto.ProductId);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<ResultDiscountCouponDto?> GetDiscountByProductIdAsync(string productId)
        {
            string query = "Select * from Coupons where ProductId=@productId and IsActive=1 and ValidDate >= GETDATE()";
            var parameters = new DynamicParameters();
            parameters.Add("@productId", productId);
            using (var connection = _context.CreateConnection())
            {
                var value = await connection.QueryFirstOrDefaultAsync<ResultDiscountCouponDto>(query, parameters);
                return value;
            }
        }

        public async Task<List<ResultDiscountCouponDto>> GetActiveProductDiscountsAsync()
        {
            string query = "Select * from Coupons where ProductId is not null and ProductId <> '' and IsActive=1 and ValidDate >= GETDATE()";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultDiscountCouponDto>(query);
                return values.ToList();
            }
        }

        public async Task SetProductDiscountAsync(string productId, int rate, DateTime validDate, bool isActive)
        {
            string checkQuery = "Select * from Coupons where ProductId=@productId";
            var parameters = new DynamicParameters();
            parameters.Add("@productId", productId);
            parameters.Add("@rate", rate);
            parameters.Add("@validDate", validDate);
            parameters.Add("@isActive", isActive);
            parameters.Add("@code", "PROD-" + productId.Substring(Math.Max(0, productId.Length - 6)).ToUpper());

            using (var connection = _context.CreateConnection())
            {
                var existing = await connection.QueryFirstOrDefaultAsync<ResultDiscountCouponDto>(checkQuery, parameters);
                if (existing != null)
                {
                    string updateQuery = "Update Coupons Set Rate=@rate, ValidDate=@validDate, IsActive=@isActive where ProductId=@productId";
                    await connection.ExecuteAsync(updateQuery, parameters);
                }
                else
                {
                    string insertQuery = "Insert Into Coupons(Code,Rate,IsActive,ValidDate,ProductId) Values(@code,@rate,@isActive,@validDate,@productId)";
                    await connection.ExecuteAsync(insertQuery, parameters);
                }
            }
        }

        public async Task DeleteDiscountByProductIdAsync(string productId)
        {
            string query = "Delete from Coupons where ProductId=@productId";
            var parameters = new DynamicParameters();
            parameters.Add("@productId", productId);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}

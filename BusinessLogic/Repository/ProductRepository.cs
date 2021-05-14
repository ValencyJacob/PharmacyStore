using BusinessLogic.Repository.IRepository;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(Product obj)
        {
            await _db.Products.AddAsync(obj);

            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(Product obj)
        {
            _db.Products.Remove(obj);

            return await SaveAsync();
        }

        public async Task<IList<Product>> GetAllAsync()
        {
            var objects = await _db.Products.Include(x => x.Company).ToListAsync();

            return objects;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var obj = await _db.Products.Include(x => x.Company).FirstOrDefaultAsync(x => x.Id == id);

            return obj;
        }

        public async Task<bool> UpdateAsync(Product obj)
        {
            _db.Products.Update(obj);

            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var obj = await _db.SaveChangesAsync();

            return obj > 0;
        }
    }
}

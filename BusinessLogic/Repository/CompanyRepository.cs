using BusinessLogic.Repository.IRepository;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(Company obj)
        {
            await _db.Companies.AddAsync(obj);

            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(Company obj)
        {
            _db.Companies.Remove(obj);

            return await SaveAsync();
        }

        public async Task<IList<Company>> GetAllAsync()
        {
            var objects = await _db.Companies.Include(x => x.Products).ToListAsync();

            return objects;
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            var obj = await _db.Companies.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);

            return obj;
        }

        public async Task<bool> UpdateAsync(Company obj)
        {
            _db.Companies.Update(obj);

            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var obj = await _db.SaveChangesAsync();

            return obj > 0;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using RoomExpenseManager.ApplicationDbContext;
using RoomExpenseManager.Interfaces;

namespace RoomExpenseManager.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly RoomExpenseContext _context;

        public GenericRepository(RoomExpenseContext context)
        {
            _context = context;
        }

        // Add entity to the database
        public async Task AddAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();  // Save changes
            }
            catch (Exception ex)
            {
                // Handle the exception (log, rethrow, etc.)
                throw new Exception("Error adding entity", ex);
            }
        }

        // Get entity by ID
        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity == null)
                {
                    throw new Exception($"Entity with ID {id} not found.");
                }

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving entity", ex);
            }
        }
        public async Task<T> GetUserIdAsync(string userId)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(userId);
                if (entity == null)
                {
                    throw new Exception($"Entity with ID {userId} not found.");
                }

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving entity", ex);
            }
        }

        // Get all entities
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();  // Fetch all records from the DbSet
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving entities", ex);
            }
        }

        // Update an existing entity
        public async Task UpdateAsync(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();  // Commit the update
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating entity", ex);
            }
        }

        // Delete entity by ID
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity == null)
                {
                    throw new Exception($"Entity with ID {id} not found.");
                }

                _context.Set<T>().Remove(entity);  // Remove entity from context
                await _context.SaveChangesAsync();  // Commit the delete operation
                return true;
            }
            catch (Exception ex)
            {
                return false;
                
            }
        }
    }

}

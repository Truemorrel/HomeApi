using System.Linq;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;
        
        public RoomRepository (HomeApiContext context)
        {
            _context = context;
        }
        
        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }
        
        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);
            
            await _context.SaveChangesAsync();
        }
        public async Task UpdateRoom(Room room, UpdateRoomQuery updateRoomQuery)
        {
            if (!string.IsNullOrEmpty(updateRoomQuery.newName))
            {
                room.Name = updateRoomQuery.newName;
            };
            if (updateRoomQuery.Area > 0)
            {
                room.Area = updateRoomQuery.Area;
            }
            if (updateRoomQuery.Voltage > 0)
            {
                room.Voltage = updateRoomQuery.Voltage;
            }
            room.GasConnected = updateRoomQuery.GasConnected;

            // Добавляем в базу 
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
            {
                _context.Rooms.Update(room);
            }

            // Сохраняем изменения в базе 
            await _context.SaveChangesAsync();
        }
    }
}
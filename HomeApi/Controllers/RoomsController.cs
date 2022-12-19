using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;
using HomeApi.Data.Queries;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _roomRepository;
        private IDeviceRepository _deviceRepository;
        private IMapper _mapper;

        public RoomsController(IRoomRepository roomRepository, IDeviceRepository deviceRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _deviceRepository = deviceRepository;
            _mapper = mapper;
        }

        //TODO: Задание - добавить метод на получение всех существующих комнат

        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _roomRepository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _roomRepository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }

            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }
        [HttpPut]
        [Route("{name}")]
        public async Task<IActionResult> Edit(
            [FromRoute] string name,
            [FromBody] RoomUpdateRequest request)
        {
            var roomToChange = await _roomRepository.GetRoomByName(name);
            if (roomToChange == null)
            {
                return StatusCode(400, $"Ошибка: \'{name}\' не существует.");
            };
            var roomDevices = _deviceRepository.GetDevices().Result.Where(d => d.RoomId == roomToChange.Id).ToArray();
            var needGas = roomDevices.Where(d => d.GasUsage && !request.GasSupply ); //  == Gas.notConnected
            var needPropperCurrent = roomDevices.Where(d => d.CurrentVolts != request.Voltage);
            var devicesForReplace = needGas.Union(needPropperCurrent).ToArray();

            var resp = new GetEquipmentResponse()
            {
                DeviceAmount = devicesForReplace.Length,
                Devices = _mapper.Map<Device[], DeviceView[]>(devicesForReplace),
            };
            if (resp.Devices.Length > 0)
            {
                return BadRequest(resp);
            }
            else
            {
                await _roomRepository.UpdateRoom(
                    roomToChange,
                    new UpdateRoomQuery()
                    {
                        Area = request.Area,
                        GasConnected = request.GasSupply , // (request.GasSupply == Gas.connected) ? true : ((request.GasSupply == Gas.notConnected) ? false : roomToChange.GasConnected)
                        newName = request.Name,
                        Voltage = request.Voltage,
                    });
                return StatusCode(200, resp);
            }
        }
    }
}
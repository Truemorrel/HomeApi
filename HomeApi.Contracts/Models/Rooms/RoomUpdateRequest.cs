using HomeApi.Contracts.Models.Devices;
using HomeApi;
using System.ComponentModel.DataAnnotations;

namespace HomeApi.Contracts.Models.Rooms
{
    public class GetEquipmentResponse
    {
        public int DeviceAmount { get; set; }
        public DeviceView[] Devices { get; set; }
    }
    public class RoomUpdateRequest
    {
        //public Guid Id { get; set; } = Guid.NewGuid();
        //public DateTime AddDate { get; set; } = DateTime.Now;
        [Required]
        public string Name { get; set; }
        public int Area { get; set; }
        public bool GasSupply { get; set; }
        public int Voltage { get; set; }
    }
}

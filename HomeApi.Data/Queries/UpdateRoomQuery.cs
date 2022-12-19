using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Data.Queries
{
    public class UpdateRoomQuery
    {
        public string newName;
        public int Area;
        public bool GasConnected;
        public int Voltage;

        public UpdateRoomQuery(string newName = null,
            int Area = 0, 
            bool GasConnected = false, 
            int Voltage = 0) 
        {
            this.newName = newName;
            this.Area = Area;
            this.GasConnected = GasConnected;
            this.Voltage = Voltage;
        }
    }
}

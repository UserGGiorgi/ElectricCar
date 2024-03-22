using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CoolElectricCar
{
    public class ElectricCar
    {
        public ElectricCar(int Blevel)
        {
            BatteryLevel = Blevel;
        }
        private int _batteryLevel;
        public int BatteryLevel
        {
            get
            { return _batteryLevel; }
            set
            {
                if (value > -1 && value < 101) { _batteryLevel = value; }
                else { _batteryLevel = 100; }
            }
        }
        public string Model { get; set; }
        public int Year { get; set; }

        public async Task Charge(CancellationToken token)
        {
           
           while (BatteryLevel < 100)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(10000);
                BatteryLevel += 5;
            }
        }
    }

}

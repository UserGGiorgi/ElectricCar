using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
namespace CoolElectricCar
{
    class program
    {
         static async Task Main(string[] args)
        {
            CancellationTokenSource source= new CancellationTokenSource();
            var token=source.Token;
            ElectricCar car1 = new ElectricCar(99);
            ElectricCar car2 = new ElectricCar(90);
            ElectricCar car3 = new ElectricCar(93);
            ElectricCar car4 = new ElectricCar(98);
            ElectricCar car5 = new ElectricCar(87);
            IEnumerable<ElectricCar> cars = new ElectricCar[] { car1, car2, car3, car4, car5 };
            Stopwatch sw = new Stopwatch();
            Task charging = Task.Run(() => ChargeTogether(cars, token));
            Task cancel = Task.Run(() =>
            {
                Console.ReadKey();
                source.Cancel();
            });
            sw.Start();
            await charging;
            sw.Stop();
            double timer = sw.ElapsedMilliseconds / 1000;

            bool AllCharged = true;
            foreach(var car in cars)
            {
                if(car.BatteryLevel!=100)
                {
                    AllCharged = false;
                }
             }
            if(AllCharged)
            {
                Console.WriteLine("Cars Are Fully Charged");
                Console.WriteLine($"It Took {timer} Seconds To Charge");
            }
            
        }
        static async Task ChargeTogether(IEnumerable<ElectricCar> cars,CancellationToken token)
        {
            int Numeracion = 1;
            Console.WriteLine("Charging Has Been Started");
            List<Task> tasks = new List<Task>();
            foreach (var car in cars)
            {
                    Console.WriteLine("Car{0} Is Charging", Numeracion);
                    Numeracion++;
                    ElectricCar CurrentCar = car;
                    Task task = Task.Run(() => CurrentCar.Charge(token), token);
                    tasks.Add(task);
                    token.ThrowIfCancellationRequested();
            }
            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex )
            {
                Console.WriteLine("------------------\nCharging Is Canceled!!!!\n_____________________" );
            }
        }      
    }
}



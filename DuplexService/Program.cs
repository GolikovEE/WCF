using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DuplexService
{
    [ServiceContract(CallbackContract = typeof(IClientCallback))]
    public interface IStockService
    {
        [OperationContract(IsOneWay = true)]
        void RegisterForUpdates(string ticker);
    }
    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void PriceUpdate(string ticker, double price);
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (ServiceHost serviceHost = new ServiceHost(typeof(StockService)))
                {
                    serviceHost.Open();
                    Console.WriteLine("Сервис запущен...");

                    Console.WriteLine("Для завершения нажмите <Enter>");
                    Console.ReadLine();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(">");
            Console.ReadLine();
        }
    }
}

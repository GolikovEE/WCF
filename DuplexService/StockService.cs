using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DuplexService
{
    public class StockService : IStockService
    {
        public class Worker 
        {
            public string ticker;
            public Update workerProcess;
        }

        public static Hashtable workers = new Hashtable();

        //----------------------------------------------------------------------------------------- IStockService
        public void RegisterForUpdates(string ticker)
        {
            Worker w = null;
            // при необходимости создаём новый рабочий объект, добавляем его
            // в хэш-таблицу и запускаем в отдельном потоке
            if (!workers.ContainsKey(ticker))
            {
                w = new Worker();
                w.ticker = ticker;
                w.workerProcess = new Update();
                w.workerProcess.ticker = ticker;
                workers[ticker] = w;
                Thread t = new Thread(new ThreadStart(w.workerProcess.SendUpdateToClient));
                t.IsBackground = true;
                t.Start();
            }
            // получить рабочий объект для данного тикера и добавить 
            // прокси клиента в список обработанных вызовов
            w = (Worker)workers[ticker];
            IClientCallback c = OperationContext.Current.GetCallbackChannel<IClientCallback>();
            lock (w.workerProcess.callbacks)
                w.workerProcess.callbacks.Add(c);
        }
    }

    public class Update
    {
        public string ticker;
        public List<IClientCallback> callbacks = new List<IClientCallback>();

        public void SendUpdateToClient()
        {
            Random w = new Random();
            Random p = new Random();
            while (true)
            {
                Thread.Sleep(w.Next(5000)); // откуда-то получаем обновления

                lock (callbacks)
                    foreach (IClientCallback c in callbacks)
                    {
                        try
                        {
                            c.PriceUpdate(ticker, 100.00 + p.NextDouble() * 10);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при отправке кэшированного значения клиенту: {ex.Message}");
                        }
                    }
            }
        }
    }
}

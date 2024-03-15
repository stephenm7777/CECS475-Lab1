using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Stock
{
    public class StockBroker
    {
        public string BrokerName { get; set; }
        public List<Stock> stocks = new List<Stock>();
        public static ReaderWriterLockSlim myLock = new ReaderWriterLockSlim();
        readonly string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lab1_output.txt");
        string titles = "Broker".PadRight(16) + "Stock".PadRight(16) + "Value".PadRight(16) + "Changes".PadRight(10) + "Date and Time";

        public StockBroker(string brokerName)
        {
            BrokerName = brokerName;
        }

        public void AddStock(Stock stock)
        {
            stocks.Add(stock);
            stock.StockEvent += EventHandler;
        }

        void EventHandler(object sender, StockNotification e)
        {
            try
            {
                myLock.EnterWriteLock();

                Stock newStock = (Stock)sender;

                Console.WriteLine($"{BrokerName.PadRight(16)}{e.StockName.PadRight(16)}{e.CurrentValue.ToString().PadRight(16)}{e.NumChanges.ToString().PadRight(10)}{DateTime.Now}");

                using (StreamWriter outputFile = new StreamWriter(destPath, true))
                {
                    outputFile.WriteLine($"{BrokerName.PadRight(16)}{e.StockName.PadRight(16)}{e.CurrentValue.ToString().PadRight(16)}{e.NumChanges.ToString().PadRight(10)}{DateTime.Now}");
                }
            }
            finally
            {
                myLock.ExitWriteLock();
            }
        }
    }
}
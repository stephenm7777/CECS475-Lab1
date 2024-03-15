using System;
using System.Threading;

namespace Stock
{
    public class Stock
    {
        public event EventHandler<StockNotification> StockEvent;

        private string _name;
        private int _initialValue;
        private int _maxChange;
        private int _threshold;
        private int _numChanges;
        private int _currentValue;
        private readonly Thread _thread;

        public string StockName { get => _name; set => _name = value; }
        public int InitialValue { get => _initialValue; }
        public int CurrentValue { get => _currentValue; }
        public int MaxChange { get => _maxChange; }
        public int Threshold { get => _threshold; }
        public int NumChanges { get => _numChanges; }

        public Stock(string name, int startingValue, int maxChange, int threshold)
        {
            _name = name;
            _initialValue = startingValue;
            _currentValue = InitialValue;
            _maxChange = maxChange;
            _threshold = threshold;
            _numChanges = 0;

            _thread = new Thread(new ThreadStart(Activate));
            _thread.Start();
        }

        public void Activate()
        {
            for (int i = 0; i < 25; i++)
            {
                Thread.Sleep(500);
                ChangeStockValue();
            }
        }

        public void ChangeStockValue()
        {
            var rand = new Random();
            _currentValue += rand.Next(1, _maxChange + 1);
            _numChanges++;

            if (Math.Abs(_currentValue - _initialValue) > _threshold)
            {
                OnStockEvent(new StockNotification(_name, _currentValue, _numChanges));
            }
        }

        protected virtual void OnStockEvent(StockNotification e)
        {
            StockEvent?.Invoke(this, e);
        }
    }
}

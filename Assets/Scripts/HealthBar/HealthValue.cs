namespace HealthBar
{
    public class HealthValue
    {
        private int _maxValue;
        private int _step;
        private int _currentValue;
        private int _takenHitsCount = 0;

        public HealthValue(int maxValue, int step)
        {
            _maxValue = maxValue;
            _step = step;
            _currentValue = maxValue;
        }

        public void Decrease()
        {
            RecordHitTaken();
            DecreaseHealthByStep();
        }

        private void DecreaseHealthByStep()
        {
            if (_currentValue - _step > 0)
            {
                _currentValue -= _step;
                return;
            }

            _currentValue = 0;

            throw new HealthValueReachedZeroException();
        }

        private void RecordHitTaken()
        {
            _takenHitsCount++;
        }

        public int GetTakenHitsCount()
        {
            return _takenHitsCount;
        }

        public bool ReachedZero()
        {
            return _currentValue == 0;
        }
    }
}
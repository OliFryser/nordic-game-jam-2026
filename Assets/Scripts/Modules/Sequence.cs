using System;

namespace Modules
{
    public class Sequence
    {
        public  int[] Numbers { get; }
        private int _index;
        public Action OnComplete;
        
        public Sequence(int[] numbers)
        {
            Numbers = numbers;
        }

        public void Enter(int input)
        {
            if (input == Numbers[_index])
            {
                _index++;
            }
            else if (input == Numbers[0])
            {
                _index = 1;
            }
            else
            {
                _index = 0;
            }

            if (_index == Numbers.Length)
            {
                OnComplete?.Invoke();
            }
        }
    }
}
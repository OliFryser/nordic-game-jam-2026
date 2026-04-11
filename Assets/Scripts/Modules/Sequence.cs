using System;
using System.Linq;
using UnityEngine;

namespace Modules
{
    [Serializable]
    public class Sequence
    {
        public int[] RelativeSequenceNumbers { get; }
        public int Length => _sequenceNumbers.Length;
        [SerializeField]
        private int[] _sequenceNumbers;
        private int SequenceProgress { get; set; }

        public bool IsCompleted => SequenceProgress == _sequenceNumbers.Length;
        
        public Sequence(int startIndex, int[] sequenceNumbers)
        {
            SequenceProgress = 0;
            RelativeSequenceNumbers = sequenceNumbers;
            _sequenceNumbers = sequenceNumbers.Select(s => s + startIndex).ToArray();
        }

        public void PrintRemainingSequence()
        {
            string sequence = $"Enter sequence: ";
            for (int i = SequenceProgress; i < _sequenceNumbers.Length; i++)
            {
                sequence += $"{_sequenceNumbers[i]} ";
            }
            Debug.Log(sequence);
        }

        public void EnterInSequence(int number)
        {
            if (number == _sequenceNumbers[SequenceProgress])
            {
                SequenceProgress++;
            }
            // Player pressed first combination
            else if (number == _sequenceNumbers[0])
            {
                SequenceProgress = 1;
            }
            // Restart sequence
            else
            {
                SequenceProgress = 0;
            }
        }
    }
}
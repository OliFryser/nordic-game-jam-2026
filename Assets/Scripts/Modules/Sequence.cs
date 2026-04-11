using System;
using Input;
using UnityEngine;
using UnityEngine.Events;

namespace Modules
{
    [Serializable]
    public class Sequence
    {
        [SerializeField]
        private DashboardSection _dashboardSection;
        [SerializeField] private int[] _relativeSequenceNumbers;

        public int[] RelativeSequenceNumbers => _relativeSequenceNumbers; 
        public int Length => RelativeSequenceNumbers.Length;
        private int SequenceProgress { get; set; }
        
        private Action OnSequenceComplete { get; set; }
        
        
        public Sequence(DashboardSection section, int[] sequenceNumbers)
        {
            SequenceProgress = 0;
            _relativeSequenceNumbers = sequenceNumbers;
        }

        public void PrintRemainingSequence()
        {
            string sequence = "Enter sequence: ";
            for (int i = SequenceProgress; i < RelativeSequenceNumbers.Length; i++)
            {
                sequence += $"{RelativeSequenceNumbers[i]} ";
            }
            Debug.Log(sequence);
        }

        public void EnterInSequence(int number)
        {
            int targetNumber = GetAbsoluteNumber(SequenceProgress);
            if (number == targetNumber)
            {
                SequenceProgress++;
            }
            // Player pressed first in combination
            else if (number == GetAbsoluteNumber(0))
            {
                SequenceProgress = 1;
            }
            // Restart sequence
            else
            {
                SequenceProgress = 0;
            }

            if (SequenceProgress >= RelativeSequenceNumbers.Length)
            {
                OnSequenceComplete?.Invoke();
            }
        }

        public int GetAbsoluteNumber(int sequenceIndex)
            => RelativeSequenceNumbers[sequenceIndex] 
                   + new DashboardLayout().GetSectionStartFromDashboardSection(_dashboardSection);

        public void AddOnSequenceCompleteListener(Action onSequenceComplete)
        {
            OnSequenceComplete += onSequenceComplete;
        }
    }
}
using System;
using Input;
using UnityEngine;

namespace Modules
{
    [Serializable]
    public class Sequence
    {
        public int StartingIndex => DashboardLayout.GetSectionStartFromName(SectionName);
        
        public int[] _relativeIndices;

        private int SequenceProgress
        {
            get; 
            set;
        }

        public bool IsCompleted => SequenceProgress == _relativeIndices.Length;
        public string SectionName { get; set; }
        
        public Sequence(string section, int[] relativeIndices)
        {
            SectionName = section;
            SequenceProgress = 0;
            _relativeIndices = relativeIndices;
        }


        public void PrintSequence()
        {
            string sequence = $"Enter sequence in {SectionName}: ";
            foreach (var relativeIndex in _relativeIndices)
            {
                sequence += $"{relativeIndex} ";
            }
            Debug.Log(sequence);
        }

        public void PrintRemainingSequence()
        {
            string sequence = $"Enter sequence in {SectionName}: ";
            for (int i = SequenceProgress; i < _relativeIndices.Length; i++)
            {
                sequence += $"{_relativeIndices[i]} ";
            }
            Debug.Log(sequence);
        }

        public void EnterInSequence(int index)
        {
            if (IsCompleted)
            {
                Debug.Log("Sequence is completed");
                return;
            }
            
            int currentRequest = StartingIndex + _relativeIndices[SequenceProgress];
            Debug.Log("Current Request: " + currentRequest + " Current input index: " + index);
            if (currentRequest == index)
            {
                SequenceProgress += 1;
            }
            else
            {
                SequenceProgress = 0;
            }
        }
    }
}
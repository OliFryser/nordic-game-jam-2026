using System;
using UnityEngine;

namespace Modules
{
    [CreateAssetMenu(fileName = "SequenceOnCompleteEmitter", menuName = "SequenceOnCompleteEmitter", order = 0)]
    public class SequenceOnCompleteEmitter : ScriptableObject
    {
        public Action OnSequenceComplete;
    }
}
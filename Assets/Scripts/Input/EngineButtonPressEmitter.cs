using System;
using UnityEngine;

namespace Input
{
    [CreateAssetMenu(fileName = "EngineButtonPressEmitter", menuName = "Engine Button Press Emitter", order = 0)]
    public class EngineButtonPressEmitter : ScriptableObject
    {
        public Action<EngineButton> OnPress;
        public Action<EngineButton> OnRelease;
    }
}
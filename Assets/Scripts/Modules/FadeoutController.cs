using System;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Modules
{
    public class FadeoutController : MonoBehaviour
    {
        [SerializeField] private Image _whiteScreen;
        [SerializeField] private float _fadeoutTime = 10f;

        public void FadeOut()
        {
            Color startColor = new Color(1, 1, 1, 0);
            Color endColor = Color.white;

            LMotion.Create(startColor, endColor, 10f)
                .WithEase(Ease.Linear)
                .WithDelay(2f)
                .BindToColor(_whiteScreen);
        }
    }
}
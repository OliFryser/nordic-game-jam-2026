using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Modules
{
    public class Lever : MonoBehaviour
    {
        [SerializeField] private Transform _leverHandle;
        [SerializeField] private float _duration = .5f;

        [SerializeField] private Transform _start;
        [SerializeField] private Transform _end;
        [SerializeField] private SoundManager _soundManager;

        [SerializeField] private FadeoutController _fadeoutController;
        
        
        private bool _hasBeenPushed;
        
        public void Push()
        {
            if (_hasBeenPushed) return;
            
            _hasBeenPushed = true;
            
            LMotion.Create(_start.position, _end.position, _duration)
                .WithEase(Ease.InQuint) 
                .BindToPosition(_leverHandle);
            
            LMotion.Create(_start.rotation, _end.rotation, _duration)
                .WithEase(Ease.InQuint) 
                .BindToRotation(_leverHandle);
            
            _soundManager.Play(Sound.Alarm);
            
            _fadeoutController.FadeOut();
        }
    }
}
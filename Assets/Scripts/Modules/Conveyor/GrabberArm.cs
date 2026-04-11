using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Modules.Conveyor
{
    public class GrabberArm : MonoBehaviour
    {
        [SerializeField] private Transform _grabberArmExtender;
        [SerializeField] private float _extendedHeight;
        [SerializeField] private float _retractedHeight;
        
        public void ExtendArm()
        {
            LMotion.Create(_retractedHeight, _extendedHeight, .2f)
                .WithEase(Ease.InOutQuad)
                .BindToLocalPositionY(_grabberArmExtender);
        }

        public void RetractArm(Transform grabbedItem)
        {
            LMotion.Create(_extendedHeight, _retractedHeight, .2f)
                .WithEase(Ease.InOutQuad)
                .BindToLocalPositionY(_grabberArmExtender);
            LMotion.Create(grabbedItem.position.y, grabbedItem.position.y + _retractedHeight, .2f)
                .WithEase(Ease.InOutQuad)
                .BindToPositionY(grabbedItem);
        }
    }
}

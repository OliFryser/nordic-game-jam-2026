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
            LMotion.Create(_grabberArmExtender.position.y, _extendedHeight, 0.2f)
                .WithEase(Ease.InOutQuad)
                .BindToPositionY(_grabberArmExtender);
        }

        public void RetractArm(Transform grabbedItem)
        {
            LMotion.Create(_grabberArmExtender.position.y, _retractedHeight, 0.2f)
                .WithEase(Ease.InOutQuad)
                .BindToPositionY(_grabberArmExtender);
            LMotion.Create(_grabberArmExtender.position.y, _retractedHeight, 0.2f)
                .WithEase(Ease.InOutQuad)
                .BindToPositionY(grabbedItem);
        }
    }
}

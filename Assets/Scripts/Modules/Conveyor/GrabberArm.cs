using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Modules.Conveyor
{
    public class GrabberArm : MonoBehaviour
    {
        [SerializeField] private Renderer _box;
        [SerializeField] private Transform _grabberArmExtender;
        [SerializeField] private float _extendedHeight;
        [SerializeField] private float _retractedHeight;
        [SerializeField] private float _extendFailHeight = 1f;

        public void SetMaterialOnBox(Material material)
        {
            _box.material = material;
        }
        
        public void ExtendArm(float grabberSpeed)
        {
            LMotion.Create(_retractedHeight, _extendedHeight, grabberSpeed)
                .WithEase(Ease.InOutQuad)
                .BindToLocalPositionY(_grabberArmExtender);
        }

        public void RetractArm(Transform grabbedItem, float grabberSpeed)
        {
            LMotion.Create(_extendedHeight, _retractedHeight, grabberSpeed)
                .WithEase(Ease.InOutQuad)
                .BindToLocalPositionY(_grabberArmExtender);
            LMotion.Create(grabbedItem.position.y, grabbedItem.position.y + _retractedHeight, grabberSpeed)
                .WithEase(Ease.InOutQuad)
                .BindToPositionY(grabbedItem);
        }

        public void ExtendFailArm(float grabberSpeed)
        {
            LMotion.Create(_retractedHeight, _extendFailHeight, grabberSpeed)
                .WithEase(Ease.InOutQuad)
                .BindToLocalPositionY(_grabberArmExtender);
        }

        public void RetractFailArm(float grabberSpeed)
        {
            LMotion.Create(_extendFailHeight, _retractedHeight, grabberSpeed)
                .WithEase(Ease.InOutQuad)
                .BindToLocalPositionY(_grabberArmExtender);
        }
    }
}

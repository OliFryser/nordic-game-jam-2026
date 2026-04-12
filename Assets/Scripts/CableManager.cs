using UnityEngine;

public class CableManager : MonoBehaviour
{
   [SerializeField] private Material _cableDefaultMaterial;
   [SerializeField] private Material _cableActiveMaterial;

   public void SetCableActive(Renderer cable)
   {
      cable.material = _cableActiveMaterial;
   }

   public void SetCableDefault(Renderer cable)
   {
      cable.material = _cableDefaultMaterial;
   }

}
using UnityEngine;

public class CableManager : MonoBehaviour
{
   [SerializeField] private Material _cableDefaultMaterial;
   [SerializeField] private Material _cableActiveMaterial;

   public async void TurnOnCable(Renderer cable, float duration)
   {
      cable.material = _cableActiveMaterial;
      await Awaitable.WaitForSecondsAsync(duration);
      cable.material = _cableDefaultMaterial;
   }

}
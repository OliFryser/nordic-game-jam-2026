using LitMotion;
using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [SerializeField] private float intensity = 0.05f; // How far it moves
    [SerializeField] private float speed = 1.5f;     // How fast it wiggles

    void Start()
    {
        // Wiggle on X axis
        LMotion.Create(-intensity, intensity, speed)
            .WithEase(Ease.InOutSine)
            .WithLoops(-1, LoopType.Yoyo) // Infinite back-and-forth
            .WithDelay(Random.value)      // Desync X and Y
            .Bind(x => 
            {
                var pos = transform.localPosition;
                pos.x = x;
                transform.localPosition = pos;
            });

        // Wiggle on Y axis
        LMotion.Create(-intensity, intensity, speed * 0.8f) // Different speed for realism
            .WithEase(Ease.InOutSine)
            .WithLoops(-1, LoopType.Yoyo)
            .Bind(y => 
            {
                var pos = transform.localPosition;
                pos.y = y;
                transform.localPosition = pos;
            });
    }
}
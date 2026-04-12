using LitMotion;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    [SerializeField] private float maxIntensity = 0.5f;
    [SerializeField] private float rampUpDuration = 5f;
    [SerializeField] private float shakeDuration = 10f;

    private float currentIntensity = 0f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake()
    {
        // 1. Ramp up the intensity slowly
        LMotion.Create(0f, maxIntensity, rampUpDuration)
            .WithEase(Ease.InQuad) // Use InQuad so it feels like it's "building"
            .Bind(v => currentIntensity = v)
            .AddTo(gameObject);

        // 2. The actual shaking logic (High frequency)
        // We run this every frame to create the jitter
        LMotion.Create(0f, 1f, rampUpDuration + shakeDuration)
            .Bind(_ =>
            {
                // Generate a random point inside a sphere and scale by current intensity
                Vector3 shakeOffset = Random.insideUnitSphere * currentIntensity;
                transform.localPosition = originalPosition + shakeOffset;
            })
            .AddTo(gameObject);
    }
}
using System;
using Modules;
using UnityEngine;
using Random = UnityEngine.Random;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject _sparks;
    [SerializeField] private GameObject _smoke;
    
    private Battery _battery;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Transform[] _smokeSpawnPoints;
    
    public void Initialize(Battery battery)
    {
        _battery = battery;
    }

    private void Start()
    {
        _ = Spark();
        _ = Smoke();
    }

    private async Awaitable Smoke()
    {
        while (true)
        {
            Vector3 position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
            Instantiate(_sparks, position, _sparks.transform.rotation);
            float waitTime = (1 - _battery.Charge + .2f) * 5f;
            // float waitTime = .5f;
            print($"Sparking. Waiting for {waitTime} seconds");
            await Awaitable.WaitForSecondsAsync(waitTime);
        }
    }

    private async Awaitable Spark()
    {
        while (true)
        {
            Vector3 position = _smokeSpawnPoints[Random.Range(0, _smokeSpawnPoints.Length)].position;
            Instantiate(_smoke, position, _smoke.transform.rotation);
            float waitTime = (1 - _battery.Charge + .2f) * 20f;
            // float waitTime = .5f;
            print($"Sparking. Waiting for {waitTime} seconds");
            await Awaitable.WaitForSecondsAsync(waitTime);
        }
    }
}
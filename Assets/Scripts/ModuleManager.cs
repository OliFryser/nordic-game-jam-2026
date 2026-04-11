using System.Collections.Generic;
using Input;
using Modules;
using UnityEngine;

public class ModuleManager : MonoBehaviour
{
    private int ModuleIndex { get; set; } = 0;
    
    [SerializeField] private List<Sequence> _sequences = 
        new()
        {
            new Sequence(DashboardSection.Lights, new int[]{ 0, 1, 5 }),
            new Sequence(DashboardSection.Lights, new int[]{ 0, 3, 5 }),
        };

    public void Start()
    {
    }
    
    public void Tick(EngineButton engineButton)
    {
        var currentModule = _sequences[ModuleIndex];
        currentModule.EnterInSequence(engineButton.ButtonIndex);

        // if (currentModule.IsCompleted)
        // {
        //     ModuleIndex++;
        //     Debug.Log($"Module Complete");
        // }
        // else
        // {
        //     currentModule.PrintRemainingSequence();
        // }
        //
        // if (ModuleIndex >= _sequences.Count)
        // {
        //     Debug.Log($"All Puzzles Completed");
        // }
    }
}
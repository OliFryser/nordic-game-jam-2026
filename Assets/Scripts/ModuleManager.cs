using System.Collections.Generic;
using Input;
using Modules;
using UnityEngine;

public class ModuleManager : MonoBehaviour
{
    private int ModuleIndex { get; set; } = 0;
    
    [SerializeField] private List<Sequence> Sequences = 
        new()
        {
            new Sequence("Lights", new int[]{ 0, 1, 5 }),
            new Sequence("Engine", new int[]{ 0, 3, 5 }),
        };

    public void Start()
    {
        Sequences[ModuleIndex].PrintSequence();
    }
    
    public void Tick(EngineButton engineButton)
    {
        var currentModule = Sequences[ModuleIndex];
        currentModule.EnterInSequence(engineButton.ButtonIndex);

        if (currentModule.IsCompleted)
        {
            ModuleIndex++;
        }
        else
        {
            currentModule.PrintRemainingSequence();
        }

        if (ModuleIndex >= Sequences.Count)
        {
            Debug.Log($"All Puzzles Completed");
        }
    }
}
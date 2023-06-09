using System;
using UnityEngine;

public class AIBehaviourManager : MonoBehaviour
{
    public static AIBehaviourManager Instance { get; private set; }

    private DebugMax debugMax;

    private void Awake()
    {
        Instance = this;
    }

    public void SetDebugMax(DebugMax debugMax)
    {
        this.debugMax = debugMax;
    }

    public DebugMax GetDebugMax()
    {
        return debugMax;
    }
}

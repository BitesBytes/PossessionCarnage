using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    public event EventHandler<OnPossessedCharacterChangedEventArgs> OnPossessedCharacterChanged;
    public class OnPossessedCharacterChangedEventArgs : EventArgs
    {
        public HealthSystem healthSystem;
    }

    public void PossessedCharacterChanged(GameObject character)
    {
        OnPossessedCharacterChanged?.Invoke(this, new OnPossessedCharacterChangedEventArgs { healthSystem = character.GetComponent<HealthSystem>() });
    }

    private void Awake()
    {
        Instance = this;
    }
}

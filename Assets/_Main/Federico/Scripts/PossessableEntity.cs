using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PossessableEntity : MonoBehaviour
{
    //[SerializeField] protected GameObject bodyPrefab;
    
    protected GameObject owner;
    public abstract void UseAbility();
    public abstract void StartPossession();
    public abstract void StopPossession();

    public abstract void SetParent(Transform obj);
}

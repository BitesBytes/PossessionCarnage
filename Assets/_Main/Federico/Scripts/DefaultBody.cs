using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBody : PossessableEntity
{
    public override void StartPossession()
    {
        gameObject.SetActive(true);
    }
    public override void StopPossession()
    {
        gameObject.SetActive(false);
    }

    public override void SetParent(Transform obj)
    {
        gameObject.transform.SetParent(obj);
    }

    public override void UseAbility()
    {
        Debug.Log("DefaultBodyAbility");
    }
}

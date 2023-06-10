using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBody : PossessableEntity
{
    public override void StartPossession()
    {
        transform.parent.gameObject.SetActive(true);
    }
    public override void StopPossession()
    {
        transform.parent.gameObject.SetActive(false);
    }
    public override void UseAbility()
    {
        Debug.Log("DefaultBodyAbility");
    }
}

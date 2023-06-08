using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeBody : EnemyBody
{
    public override void SetParent(Transform obj)
    {
        throw new System.NotImplementedException();
    }

    public override void StartPossession()
    {
    }
    public override void StopPossession()
    {
    }
    public override void UseAbility()
    {
        Debug.Log("MeleeEnemyAttack");
    }
}

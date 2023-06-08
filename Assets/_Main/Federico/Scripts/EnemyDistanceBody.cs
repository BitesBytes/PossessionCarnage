using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDistanceBody : EnemyBody
{
    public override void StartPossession()
    {

    }
    public override void StopPossession()
    {
    }
    public override void UseAbility()
    {
        Debug.Log("ArcherEnemyAttack");
    }
}

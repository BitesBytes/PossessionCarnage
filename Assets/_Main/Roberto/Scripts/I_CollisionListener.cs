using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_CollisionListener
{
    void OnCollisionEnter(Collision collision);
    void OnCollisionExit(Collision collision);
}

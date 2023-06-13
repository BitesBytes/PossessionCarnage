using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PickablePhysics))]
public class PickupObject : MonoBehaviour, I_Pickable, I_CollisionListener
{
    protected Character character;
    protected GameObject fxObject;

    public void Picked()
    {

    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Character>() && IsPlayerTrue(character))
        {
            //do something

            Picked();
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        Destroy(gameObject);
    }

    public void Update()
    {

    }

    public void OnDestroy()
    {
        if (fxObject != null)
        {
            Instantiate(fxObject, transform.position, transform.rotation);
        }
    }

    public bool IsPlayerTrue(Character character)
    {
        if (character.GetComponent<Character>().IsPlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //public void OnTriggerEnter(Collider col)
    //{
    //    //when player is true 
    //    //check for any boolean energy or health
    //    //if (IsPlayerTrue(col.GetComponent<Character>()))
    //    //{
    //    //    Picked();
    //    //}
    //}
}

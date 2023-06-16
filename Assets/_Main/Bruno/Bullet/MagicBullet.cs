using UnityEngine;

public class MagicBullet : MonoBehaviour
{

    //  Debug
    private float forceSpeed = 100f;
    private Rigidbody rigidBody;
    [HideInInspector] public GameObject RangedDebug; // mi prendo qua il ranged cosi so il forward suo per poter sparare il proiettile nella sua direzione

    private float timeToLive = 5f;
    private float timer = 0.0f;

    private bool isCast;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * 1.5f;

        if(timer >= timeToLive)
        {
            isCast = false;
            Destroy(this.gameObject);
        }

        if(!isCast)
        {
            Kinematics();
            isCast = true; // questo non fa altro che andare a bloccare il proiettile perche se si ruotera sempre quando il ranged ruota
        }


    }

    public void Kinematics() // add as event
    {
        rigidBody.velocity = RangedDebug.transform.forward; // questo è necessario sia collegato ad un evento nel animazione ma devo avere il riferimento a questa classe
    }


    void OnTriggerEnter(Collider other)
    {

    }
}

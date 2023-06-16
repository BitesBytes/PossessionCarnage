using UnityEngine;

[RequireComponent(typeof(PickupMovementEffect))]
public abstract class BasePickup : MonoBehaviour
{
    [SerializeField] protected PickupSO pickup;


    [SerializeField] protected GameObject visual;
    [SerializeField] protected GameObject visualFX;

    protected PickupMovementEffect movementEffectBehaviour;
    
    protected float healthAmount;
    protected float energyAmount;
    protected float timeForDestroying;

    protected bool isCollected;

    private float timer;

    private void Awake()
    {
        timer = pickup.TimeForDestroying;
        healthAmount = pickup.HealthAmount;
        energyAmount = pickup.EnergyAmount;
    }

    private void Start()
    {
        movementEffectBehaviour = GetComponent<PickupMovementEffect>();
    }

    private void Update()
    {
        if (isCollected)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    protected abstract void OnTriggerEnter(Collider other);
}

using UnityEngine;

public class HealthPickup : BasePickup
{
    [SerializeField] private GameObject fxObject;
    [SerializeField] private GameObject visualObject;
    [SerializeField] private float healthAmount = 50f;
    [SerializeField] private float timeForDestroying = 1f;

    private bool isCollected;

    protected override void OnTriggerEnter(Collider other)
    {
        if (!isCollected)
        {
            Player player = other.gameObject.GetComponent<Player>();

            if (player != null)
            {
                Character possessedCharacter = player.GetPossessedBodyComponent();

                if ((possessedCharacter != null) && (possessedCharacter.GetHealthSystem().IsNotMaxHealth()))
                {
                    possessedCharacter.GetHealthSystem().ChangeHealthAmount(healthAmount);
                    visualObject.SetActive(false);
                    fxObject.SetActive(true);

                    isCollected = true;
                }
            }
        }
    }

    protected override void Update()
    {
        if (isCollected)
        {
            timeForDestroying -= Time.deltaTime;
            if (timeForDestroying <= 0f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
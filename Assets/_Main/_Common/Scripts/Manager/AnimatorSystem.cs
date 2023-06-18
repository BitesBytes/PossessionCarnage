using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorSystem : MonoBehaviour
{
    private const string IS_WALKING = "isWalking";
    private const string DO_LIGHT_ATTACK = "doLightAttack";
    private const string DO_HEAVY_ATTACK = "doHeavyAttack";
    private const string DO_SPECIAL_ATTACK = "doSpecialAttack";

    private static Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public static void IsWalking(bool isWalking)
    {
        animator.SetBool(IS_WALKING, isWalking);
    }

    public static void DoLightAttack()
    {
        animator.SetTrigger(DO_LIGHT_ATTACK);
    }

    public static void DoHeavyAttack()
    {
        animator.SetTrigger(DO_HEAVY_ATTACK);
    }

    public static void DoSpecialAttack()
    {
        animator.SetTrigger(DO_SPECIAL_ATTACK);
    }

    public static bool IsAnimationFinished()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
    }
}

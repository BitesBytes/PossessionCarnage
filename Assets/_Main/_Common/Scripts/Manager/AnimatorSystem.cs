using UnityEngine;

public static class AnimatorSystem
{
    private const string IS_WALKING = "isWalking";
    private const string DO_LIGHT_ATTACK = "doLightAttack";
    private const string DO_HEAVY_ATTACK = "doHeavyAttack";
    private const string DO_SPECIAL_ATTACK = "doSpecialAttack";
    private const string STOP_ANIMATION = "stopAnimation";

    public static void IsWalking(Animator animator, bool isWalking)
    {
        animator.SetBool(IS_WALKING, isWalking);
    }

    public static void DoLightAttack(Animator animator)
    {
        animator.SetTrigger(DO_LIGHT_ATTACK);
    }

    public static void DoHeavyAttack(Animator animator)
    {
        animator.SetTrigger(DO_HEAVY_ATTACK);
    }

    public static void DoSpecialAttack(Animator animator)
    {
        animator.SetTrigger(DO_SPECIAL_ATTACK);
    }

    public static bool IsAnimationFinished(Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
    }
}

using UnityEngine;

public static class AnimatorUtils
{
    public static void PlayBoolAnimation(Animator anim, string name, bool condition)
    {
        anim.SetBool(name, condition);
    }

    public static void PlayTriggerAnimation(Animator anim, string name)
    {
        anim.SetTrigger(name);
    }
}

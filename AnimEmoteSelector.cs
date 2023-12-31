using UnityEngine;

/// <summary>
/// StateMachineBehaviour animation selector 
/// 
/// -- add this in your controller
/// public void PlaySelectedEmote()
/// {
///    animEmoteSelector = this.myAnimator.GetBehaviour<AnimEmoteSelector>();
///    animEmoteSelector.SetAnimationClip(selectedEmote);
///    myAnimator.SetBool(C.ANIM_STATE.IS_EMOTE, true);
/// }
/// </summary>
public class AnimEmoteSelector : StateMachineBehaviour
{
    private AnimationClip selectedAnimationClip = null;
    private RuntimeAnimatorController myRac = null;
    private Animator myAnimator = null;
    private bool isActive = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isActive = true;
        this.myAnimator = this.myAnimator ? this.myAnimator : animator;
        myRac = myAnimator.runtimeAnimatorController;

        AnimatorOverrideController aoc = new AnimatorOverrideController(this.myAnimator.runtimeAnimatorController);
        aoc[myAnimator.GetNextAnimatorClipInfo(0)[0].clip.name] = GetAnimationClip;
        myAnimator.runtimeAnimatorController = aoc;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.runtimeAnimatorController = myRac;
        isActive = false;
    }

    public void UpdateStateClip()
    {
        if (myAnimator == null || isActive == false) return;

        // idk why but it works only like this :/ (at least it works)
        AnimatorOverrideController aoc = new AnimatorOverrideController(this.myAnimator.runtimeAnimatorController);
        aoc[myAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name] = GetAnimationClip;
        myAnimator.runtimeAnimatorController = aoc;

        AnimatorOverrideController aoc2 = new AnimatorOverrideController(this.myAnimator.runtimeAnimatorController);
        aoc2[myAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name] = GetAnimationClip;
        myAnimator.runtimeAnimatorController = aoc2;
    }

    #region GETTER SETTER
    public AnimationClip GetAnimationClip => this.selectedAnimationClip;
    public void SetAnimationClip(AnimationClip clip) {
        this.selectedAnimationClip = clip;
        UpdateStateClip();
    } 
    #endregion
}
// @author voidstack

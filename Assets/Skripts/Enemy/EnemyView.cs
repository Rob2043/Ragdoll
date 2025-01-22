using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Animator))]
public class EnemyView : MonoBehaviour
{
    private const string IsRunningKey = "IsRunning";
    private const string BackStandingName = "BackStandingUp";
    private const string FrontStandingName = "FrontStandingUp";
    private const int DefaultLayer = -1;
    private RigAdjusterForAnimation _rigAdjusterForBackStandingUpAnimation;
    private RigAdjusterForAnimation _rigAdjusterForFrontStandingUpAnimation;
    private Transform _parent;
    private Transform _hisBone;
    private Animator animator;
    private Action _standingUpCallBack;

    private bool IsFrontUp => Vector3.Dot(_hisBone.forward, Vector3.up) < 0;
    public void Initializing(Transform parent)
    {
        animator = GetComponent<Animator>();

        _parent = parent;
        _hisBone = animator.GetBoneTransform(HumanBodyBones.Hips);

        AnimationClip[] currentClips = animator.runtimeAnimatorController.animationClips;
        Transform[] bones = _hisBone.GetComponentsInChildren<Transform>();

        _rigAdjusterForBackStandingUpAnimation = new RigAdjusterForAnimation(currentClips.First(clip => clip.name == BackStandingName), bones, this);
        _rigAdjusterForFrontStandingUpAnimation = new RigAdjusterForAnimation(currentClips.First(clip => clip.name == FrontStandingName), bones, this);
    }
    public void StartRunning() => animator.SetBool(IsRunningKey, true);
    public void PlayStandingIdle() => animator.SetBool(IsRunningKey, false);
    public void DisableAnimator() => animator.enabled = false;
    public void PlayStandingUp(Action adjustAnimationEndedCallBack, Action animationEndedCallBack = null)
    {
        AdjustParentRotationToHipsBone();
        AdjustParentPositionToHipsBone();

        _standingUpCallBack = animationEndedCallBack;

        Debug.Log(IsFrontUp);
        if (IsFrontUp == true)
            _rigAdjusterForFrontStandingUpAnimation.Adjust(() => CallbackForAdustStandingUpAnimation(FrontStandingName, adjustAnimationEndedCallBack));
        else
            _rigAdjusterForBackStandingUpAnimation.Adjust(() => CallbackForAdustStandingUpAnimation(BackStandingName, adjustAnimationEndedCallBack));
    }
    public void OnStandingUpAnimationEnded() => _standingUpCallBack?.Invoke();
    public void EnableAnimator()
    {
        animator.enabled = true;
    }
    private void CallbackForAdustStandingUpAnimation(string clipName, Action additionalCallback)
    {
        additionalCallback?.Invoke();
        animator.Play(clipName, DefaultLayer, 0f);
    }
    private void AdjustParentPositionToHipsBone()
    {
        Vector3 initHipsPosition = _hisBone.position;
        _parent.position = initHipsPosition;
        AdjustParentPositionRelativGround();
        _hisBone.position = initHipsPosition;
    }
    private void AdjustParentPositionRelativGround()
    {
        if (Physics.Raycast(_parent.position, Vector3.down, out RaycastHit hit, 5, 1 << LayerMask.NameToLayer("Ground")))
            _parent.position = new Vector3(_parent.position.x, hit.point.y, _parent.position.z);

    }
    private void AdjustParentRotationToHipsBone()
    {
        Vector3 initHipsPosition = _hisBone.position;
        Quaternion initHipsRotation = _hisBone.rotation;

        Vector3 directionForRotate = -_hisBone.up;

        if (IsFrontUp)
            directionForRotate *= -1;
        directionForRotate.y = 0;

        Quaternion correctionRotation = Quaternion.FromToRotation(_parent.forward, directionForRotate.normalized);
        _parent.rotation *= correctionRotation;

        _parent.position = initHipsPosition;
        _hisBone.rotation = initHipsRotation;
    }
}

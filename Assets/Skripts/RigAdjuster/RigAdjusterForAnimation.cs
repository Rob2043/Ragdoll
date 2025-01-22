using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigAdjusterForAnimation
{
    private const float _timeToShiftBonesToStartAnimation = 0.5f;
    private AnimationClip _clip;
    private MonoBehaviour _view;
    private List<Transform> _bones;
    private BoneTransformData[] _bonesAtStartAnimation;
    private BoneTransformData[] _bonesBeforeAnimation;
    private Coroutine _shifBonesToStandingUpAnimation;
    public RigAdjusterForAnimation(AnimationClip clip, IEnumerable<Transform> bones, MonoBehaviour view)
    {
        _clip = clip;
        _view = view;
        _bones = new List<Transform>(bones);

        _bonesBeforeAnimation = new BoneTransformData[_bones.Count];
        _bonesAtStartAnimation = new BoneTransformData[_bones.Count];

        for (int i = 0; i < _bones.Count; i++)
        {
            _bonesBeforeAnimation[i] = new BoneTransformData();
            _bonesAtStartAnimation[i] = new BoneTransformData();
        }
        SaveBoneDataFromStartAnimation();
    }
    public void Adjust(Action callback)
    {
        SaveCurrentBonesDataTo(_bonesBeforeAnimation);

        if (_shifBonesToStandingUpAnimation != null)
            _view.StopCoroutine(_shifBonesToStandingUpAnimation);

        _shifBonesToStandingUpAnimation = _view.StartCoroutine(ShiftBonesToAnimation(callback));
    }

    private IEnumerator ShiftBonesToAnimation(Action callback)
    {
        float progress = 0;
        while (progress < _timeToShiftBonesToStartAnimation)
        {
            progress += Time.deltaTime;
            float progressInPercentage = progress / _timeToShiftBonesToStartAnimation;

            for (int i = 0; i < _bones.Count; i++)
            {
                _bones[i].localPosition = Vector3.Lerp(_bonesBeforeAnimation[i].Position, _bonesAtStartAnimation[i].Position, progressInPercentage);
                _bones[i].localRotation = Quaternion.Lerp(_bonesBeforeAnimation[i].Rotation, _bonesAtStartAnimation[i].Rotation, progressInPercentage);
            }
            yield return null;
        }
        callback?.Invoke();
    }
    private void SaveCurrentBonesDataTo(BoneTransformData[] bones)
    {
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].Position = _bones[i].localPosition;
            bones[i].Rotation = _bones[i].localRotation;
        }
    }
    private void SaveBoneDataFromStartAnimation()
    {
        Vector3 initPosition = _view.transform.position;
        Quaternion intiRotation = _view.transform.rotation;

        _clip.SampleAnimation(_view.gameObject, 0);
        SaveCurrentBonesDataTo(_bonesAtStartAnimation);

        _view.transform.position = initPosition;
        _view.transform.rotation = intiRotation;
    }
}

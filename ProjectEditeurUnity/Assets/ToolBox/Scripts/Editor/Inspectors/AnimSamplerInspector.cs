using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(AnimSampler))]
public class AnimSamplerInspector : Editor
{
    private Animator _animator = null;
    private AnimationClip[] _animClipsArr = null;
    private string[] _animNamesArr = null;
    private bool _isInitialized = false;
    
    private int _currentAnimIndex = 0;
    
    private bool _isPlaying = false;

    private float _editorLastTime = 0f;

    private void OnDisable()
    {
        StopAnim();    
    }

    public override void OnInspectorGUI()
    {
        AnimSampler sampler = target as AnimSampler;
        if (sampler == null) return;

        if(!_isInitialized)
        {
            _animator = sampler.GetComponent<Animator>();
            _animClipsArr = FindAnimClips(_animator);
            _animNamesArr = FindAnimNames(_animClipsArr);
            _isInitialized = true;
        }

        _currentAnimIndex = EditorGUILayout.Popup("Current Anim", _currentAnimIndex, _animNamesArr);

        if(_isPlaying)
        {
            if(GUILayout.Button("Stop"))
            {
                StopAnim();
            }
        } else
        {
            if(GUILayout.Button("Play"))
            {
                PlayAnim();
            }
        }
    }

    private void PlayAnim()
    {
        if (_isPlaying) return;
        _editorLastTime = Time.realtimeSinceStartup;
        EditorApplication.update += _OnEditorUpdate;
        AnimationMode.StartAnimationMode();
        _isPlaying = true;
    }

    

    private void StopAnim()
    {
        if (!_isPlaying) return;
        EditorApplication.update -= _OnEditorUpdate;
        AnimationMode.StopAnimationMode();
        _isPlaying = false;
    }

    private void _OnEditorUpdate()
    {
        if (!_isPlaying) return;
        float animTime = Time.realtimeSinceStartup - _editorLastTime;
        AnimationClip animClip = _animClipsArr[_currentAnimIndex];
        animTime %= animClip.length;
        AnimationMode.SampleAnimationClip(_animator.gameObject, animClip, animTime);
    }

    private string[] FindAnimNames(AnimationClip[] animClipsArr)
    {
        List<string> resultList = new List<string>();
        foreach(AnimationClip clip in animClipsArr)
        {
            resultList.Add(clip.name);
        }

        return resultList.ToArray();
    }

    private AnimationClip[] FindAnimClips(Animator animator)
    {
        List<AnimationClip> resultList = new List<AnimationClip>();

        AnimatorController editorController = animator.runtimeAnimatorController as AnimatorController;

        AnimatorControllerLayer controllerLayer = editorController.layers[0];
        foreach(ChildAnimatorState childState in controllerLayer.stateMachine.states)
        {
            AnimationClip animClip = childState.state.motion as AnimationClip;
            if(animClip != null)
            {
                resultList.Add(animClip);
            }
        }

        return resultList.ToArray();
    }

}

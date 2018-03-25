﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostFXHandler : MonoBehaviour {
    private PostProcessVolume _volume;
    private DepthOfField _dof;

    [SerializeField]
    [Range(.1f, 32f)]
    private float _normalAperture;
    [SerializeField]
    [Range(.1f, 32f)]
    private float _phoneAperture;
    private float _delta = 0.01f;

    private float _targetAperture;

    void Start () {
        _dof = ScriptableObject.CreateInstance<DepthOfField>();
        _dof.enabled.Override(true);
        _dof.focusDistance.Override(.1f);
        _dof.focalLength.Override(7f);
        _dof.kernelSize.Override(KernelSize.VeryLarge);
        _dof.aperture.Override(_phoneAperture);

        _volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, _dof);
        _volume.isGlobal = true;
        StartCoroutine(SpeedUp());
	}
	

	void Update () {
        _targetAperture = (GameController.IsUsingPhone || GameController.IsIntro) ? _phoneAperture : _normalAperture;

        _dof.aperture.value = Mathf.Lerp(
            _dof.aperture.value,
            _targetAperture,
            _delta);
	}

    private void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(_volume, true);
    }

    IEnumerator SpeedUp()
    {
        _delta = 0.01f;
        yield return new WaitForSeconds(5f);
        _delta = 0.2f;
    }
}

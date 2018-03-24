﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogNames : MonoBehaviour {
    Ray ray;
    RaycastHit hit;

    Dog hoverDog;

    [SerializeField]
    Text _text;

    private Camera _mainCam
    {
        get
        {
            return Camera.main.GetComponent<Camera>();
        }
    }

	void Update () {
        ray = _mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            hoverDog = hit.collider.GetComponentInParent<Dog>();
            if (hoverDog != null)
            {
                print(hoverDog.Name);
            }
        }
        else
        {
            hoverDog = null;
        }
    }

    private void OnGUI()
    {
        _text.text = (hoverDog == null) ? "" : hoverDog.Name;
    }
}
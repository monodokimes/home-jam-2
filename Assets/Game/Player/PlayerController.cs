﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IBouncer
{
    [SerializeField]
    private float _walkSpeed;

    public UnityEvent StartBouncing { get; private set; }
    public UnityEvent StopBouncing { get; private set; }

    private bool HasInput { get { return Input.GetAxis(GameTags.Horizontal) != 0; } }
    private bool _isMoving;

    private void Awake()
    {
        StartBouncing = new UnityEvent();
        StopBouncing = new UnityEvent();
    }

    // Use this for initialization
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        if (!_isMoving && HasInput)
        {
            _isMoving = true;
            StartBouncing.Invoke();
        }
        if (_isMoving && !HasInput)
        {
            _isMoving = false;
            StopBouncing.Invoke();
        }

        Walk();
    }

    private void Walk()
    {
        var movementAxis = Input.GetAxis(GameTags.Horizontal);
        var movement = new Vector3
        {
            x = movementAxis * _walkSpeed
        };

        transform.Translate(movement * Time.deltaTime);

        if (movementAxis == 0f)
            return;

        // flip the player in their movement direction
        transform.localScale = new Vector3((movementAxis < 0f) ? -1 : 1, 1, 1);
    }
}

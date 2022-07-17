using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownBumper : MonoBehaviour
{
    [SerializeField] private float amplitude = .1f;
    [SerializeField] private float speedModifier = 3;

    private float _originalY;

    private void Start()
    {
        _originalY = transform.position.y;
    }

    private void Update()
    {
        var pos = transform.position;

        transform.position = new Vector3(
            pos.x,
            (float)(_originalY + Math.Sin(Time.time * speedModifier) * amplitude),
            pos.z
        );
    }

    public void Reset()
    {
        var pos = transform.position;

        transform.position = new Vector3(
            pos.x,
            _originalY,
            pos.z
        );
    }
}
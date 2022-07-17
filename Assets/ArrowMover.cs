using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMover : MonoBehaviour
{
    [SerializeField] private GameObject arrowLeft;
    [SerializeField] private GameObject arrowRight;
    [SerializeField] private float frequency = 1;
    [SerializeField] private float amplitude = 1;

    private Vector3 _posLeftOriginal;
    private Vector3 _posRightOriginal;

    private void Start()
    {
        _posLeftOriginal = arrowLeft.transform.position;
        _posRightOriginal = arrowRight.transform.position;
    }

    private void Update()
    {
        arrowLeft.transform.position = _posLeftOriginal + Vector3.left * Math.Abs((float)(Math.Sin(Time.time * frequency) * amplitude));
        arrowRight.transform.position = _posRightOriginal + Vector3.right * Math.Abs((float)(Math.Sin(Time.time * frequency) * amplitude));
    }
}
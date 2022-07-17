using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightClickDraggable : MonoBehaviour
{
    private Camera _camera;
    private bool _currentlyDragging = false;

    private Vector3 _mouseOrig;
    private Vector3 _objectOrig;

    private float _scale;
    public float scrollMultiplier = .1f;
    public float minScale = .5f;
    public float maxScale = 2f;


    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !_currentlyDragging)
        {
            _currentlyDragging = true;
            _mouseOrig = _camera.ScreenToWorldPoint(Input.mousePosition);
            _objectOrig = transform.position;
        }

        if (Input.GetMouseButton(1) && _currentlyDragging)
        {
            Vector3 mouseCurr = _camera.ScreenToWorldPoint(Input.mousePosition);

            Vector3 mov = mouseCurr - _mouseOrig;
            mov.z = 0;

            transform.position = _objectOrig + mov;
        }

        if (Input.GetMouseButtonUp(1) && _currentlyDragging)
        {
            _currentlyDragging = false;
        }

        _scale = Mathf.Clamp(_scale + Input.mouseScrollDelta.y * scrollMultiplier, minScale, maxScale);
        transform.localScale = new Vector3(_scale, _scale, _scale);
    }
}
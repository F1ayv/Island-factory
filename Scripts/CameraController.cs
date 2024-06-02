using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private bool _isRotation;
    private bool _isFirstDirection = true;
    private Vector2 _firstPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
    }

    private void Rotation()
    {
        if(!_isRotation)
            return;

        double direction = _firstPoint.x - Input.mousePosition.x;
        
        _isFirstDirection = false;
        return;
    }

    public void SetIsRotation(bool isRotation)
    {
        if(isRotation)
            _firstPoint = Input.mousePosition;
        
        _isRotation = isRotation;
    }
}

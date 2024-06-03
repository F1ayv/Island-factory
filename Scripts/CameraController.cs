using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private bool _isRotation = false;
    private bool _isFirstDirection = true;
    private Vector2 _firstPoint;
    [SerializeField]
    private float x = 0.0f, _distanse;
    // Start is called before the first frame update
    void Start()
    {
        _distanse = (transform.position.y)/ Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.x);
        _isRotation = true;
        Rotation();
        _isRotation = false;
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

        x += Input.GetAxis("Mouse X") * 1000 * Time.deltaTime;
        UpdateCameraPosition();
        _isFirstDirection = false;
        return;
    }

    public void SetIsRotation(bool isRotation)
    {
        if(isRotation)
            _firstPoint = Input.mousePosition;
        
        _isRotation = isRotation;
    }

    void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(transform.eulerAngles.x, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0, -_distanse) + new Vector3(0,0f,0);
        position = new Vector3(position.x, 10, position.z);

        transform.rotation = rotation;
        transform.position = position;

        // Устанавливаем угол наклона ка
    }
}

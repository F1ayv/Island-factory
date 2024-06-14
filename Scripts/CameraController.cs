using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private bool _isRotation = false;
    private bool _isFirstDirection = true;
    private Vector2 _firstPoint;
    [SerializeField]
    private float x = 0.0f, _distance;
    public float mouseSensitivity = 100f; // Чувствительность мыши
    public float movementSpeed = 5f; 
    [SerializeField]
    private Vector3 lastMousePosition,saveCamPos, cameraRayCenter;
    // Start is called before the first frame update
    void Start()
    {
        FindDistance();
        _isRotation = true;
        Rotation();
        _isRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
        Move();
        Zoom();
    }

    private void Rotation()
    {
        if(!_isRotation)
            return;
        
        x += Input.GetAxis("Mouse X") * 1000 * Time.deltaTime;
        
        Quaternion rotation = Quaternion.Euler(transform.eulerAngles.x, x, 0);
        cameraRayCenter.y = 10;
        Vector3 position = rotation * new Vector3(0.0f, 0, -_distance-0.3f) + cameraRayCenter;
        position = new Vector3(position.x, 10, position.z);

        transform.rotation = rotation;
        transform.position = position;
        
        _isFirstDirection = false;
        return;
    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scroll * 10;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 4, 10);
    }

    private bool isUIClick;
    private void Move()
    {
        if(_isRotation || CreateNewBuilding.IsBuildingMode)
            return;
        
        if (Input.GetMouseButtonUp(0))
        {
            isUIClick = false;
        }

        if(isUIClick)
            return;;
        
        FindDistance();
        if (Input.GetMouseButtonDown(0))
        {
            isUIClick = EventSystem.current.IsPointerOverGameObject();
            lastMousePosition = MousePositionToWorldPoint();
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 targetPoint = MousePositionToWorldPoint();
            transform.position = new Vector3(transform.position.x - (targetPoint.x - lastMousePosition.x) , transform.position.y, transform.position.z-(targetPoint.z - lastMousePosition.z));
        }

    }

    public static Vector3 MousePositionToWorldPoint()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.nearClipPlane;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // Корректируем высоту до 0
        worldPosition.y = 0;
        return worldPosition;
    }
    
    private void FindDistance()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

        // Выпускаем луч из камеры в центр экрана
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        // Виртуальная плоскость на уровне Y=0
        Plane groundPlane = new Plane(Vector3.up, 0);

        // Переменная для хранения расстояния до пересечения с плоскостью

        // Проверяем пересечение луча с плоскостью
        if (groundPlane.Raycast(ray, out _distance))
        {
            // Получаем точку пересечения
            cameraRayCenter = ray.GetPoint(_distance);
        }

    }

    public void SetIsRotation(bool isRotation)
    {
        if(isRotation)
            _firstPoint = Input.mousePosition;
        
        _isRotation = isRotation;
    }
    
}

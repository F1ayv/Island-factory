using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Plane = UnityEngine.Plane;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CreateNewBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _selectedPrefabBuilding;
    [SerializeField] public static bool IsBuildingMode = true;
    [SerializeField] public static bool IsDeleteMode = false;
    [SerializeField] private Vector2 _sizePrefab;
    public static GameObject _flyingBuilding;
    private Plane plane;
    [SerializeField] private Sprite _buildingButtonIcon, _moveButtonIcon;
    [SerializeField] private Image _icon,_deleteImage;
    public void SelectPrefabBuilding(GameObject prefab)
    {
        _selectedPrefabBuilding = prefab;
        ITile tile = _selectedPrefabBuilding.GetComponent<ITile>();
        if(tile == null)
            _sizePrefab = Vector2.one;
        else
        {
            _sizePrefab = tile.GetSize();
        }
            
    }
    // Start is called before the first frame update
    void Start()
    {
        plane = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    private int numConv = 0;
    void Update()
    {
        if(!IsDeleteMode)
            Building();
        else
        {
            DeleteBuilding();
        }
    }

    public void Building()
    {
        if (!IsBuildingMode)
            return;

        if (_selectedPrefabBuilding == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            _flyingBuilding = Instantiate(_selectedPrefabBuilding);
            _flyingBuilding.name = $"Conveyor{numConv}";
            numConv++;
        }

        if (_flyingBuilding == null)
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (!_flyingBuilding.CompareTag("Conveyor"))
            _flyingBuilding.transform.eulerAngles = new Vector3(0, _flyingBuilding.transform.eulerAngles.y + scroll*50 * 90, 0);
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000);

        int lastHit = -1;
        for (int i = 0; i < hits.Length; i++)
        {
            Transform t = hits[i].transform;

            if (t.tag == "Ground")
            {
                if (lastHit < 0)
                    lastHit = i;
                else
                {
                    if (hits[i].distance > hits[lastHit].distance)
                        continue;
                }

                lastHit = i;
                Vector3 worldPosition = t.position;
                //Debug.Log(worldPosition);
                // Change the material of all hit colliders
                // to use a transparent shader.
                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.y);
                int z = Mathf.RoundToInt(worldPosition.z);
                _flyingBuilding.transform.position = new Vector3(x, y, z);
            }
        }

        if (lastHit < 0)
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            Ray ray1 = Camera.main.ScreenPointToRay(mouseScreenPosition);
            float distance;
            if (plane.Raycast(ray1, out distance))
            {
                Vector3 point = ray1.GetPoint(distance);
                _flyingBuilding.transform.position = new Vector3(point.x, 0, point.z);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (lastHit < 0)
            {
                Destroy(_flyingBuilding);
            }
            else
            {
                ITile tile = _flyingBuilding.GetComponent<ITile>();
                if(tile != null)
                    if(tile.CanInstall())
                    {
                        tile.CheckPos();
                    }
                else
                    {
                        Destroy(_flyingBuilding);
                    }
            }
            _flyingBuilding = null;
        }
    }

    public void DeleteBuilding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 1000);

            foreach (var hit in hits)
            {
                if (!hit.transform.CompareTag("Ground"))
                {
                    Tile t = hit.transform.GetComponent<Tile>();
                    if (t != null)
                    {
                        Destroy(t.gameObject);
                        break;
                    }
                }
            }
        }
    }

    public void SetIsDeleteMode()
    {
        IsDeleteMode = !IsDeleteMode;

        if (!IsDeleteMode)
        {
            _deleteImage.color = new Color(0.6320754f,0.6320754f,0.6320754f);
        }
        else
        {
            _deleteImage.color = new Color(0.735849f,0.410904f,0.07289071f);
        }
    }

    public void SetBuidingMode()
    {
        IsBuildingMode = !IsBuildingMode;

        if (IsBuildingMode)
        {
            _deleteImage.gameObject.SetActive(true);
            _icon.sprite = _buildingButtonIcon;
        }
        else
        {
            _deleteImage.gameObject.SetActive(false);
            if (IsDeleteMode)
                SetIsDeleteMode();
            _icon.sprite = _moveButtonIcon;
        }
    }
}

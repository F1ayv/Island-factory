using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using Plane = UnityEngine.Plane;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CreateNewBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _selectedPrefabBuilding;
    [SerializeField] public static bool IsBuildingMode = true;
    [SerializeField] private Vector2 _sizePrefab;
    GameObject _flyingBuilding;
    private Plane plane;
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
        Building();
        TapToBuilding();
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

    public void TapToBuilding()
    {
        
    }

    public void SetBuidingMode()
    {
        IsBuildingMode = !IsBuildingMode;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateNewBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _selectedPrefabBuilding;
    [SerializeField] private bool _isBuildingMode = true;
    [SerializeField] private Vector2 _sizePrefab;
    GameObject _flyingBuilding;
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
        
    }

    // Update is called once per frame
    private int numConv = 0;
    void Update()
    {
        if (_isBuildingMode)
        {
            if(_selectedPrefabBuilding == null)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                if(EventSystem.current.IsPointerOverGameObject())
                    return;
                
                _flyingBuilding = Instantiate(_selectedPrefabBuilding);
                _flyingBuilding.name = $"Conveyor{numConv}";
                numConv++;
            }
            
            if(_flyingBuilding == null)
                return;
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);
                //Vector2 sizeBuilding = _flyingBuilding.GetSize();
                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);
                _flyingBuilding.transform.position = new Vector3(x,0,y);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_flyingBuilding.transform.tag == "ResurseGiver")
                {
                    
                }

                if (_flyingBuilding.transform.tag == "Conveyor")
                {
                    Conveyor conveyor = _flyingBuilding.GetComponent<Conveyor>();
                    conveyor.ChecPos();
                }

                if (_flyingBuilding.transform.GetChild(0).tag == "Mekanism")
                {

                }

                _flyingBuilding = null;
            }
        }
    }
}

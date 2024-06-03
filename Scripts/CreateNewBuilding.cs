using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateNewBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _selectedPrefabBuilding;
    [SerializeField] public static bool IsBuildingMode = true;
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
        if (IsBuildingMode)
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
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray,1000);

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
                        if(hits[i].distance > hits[lastHit].distance)
                            continue;
                    }
                    
                    lastHit = i;
                    Vector3 worldPosition = t.position;
                    Debug.Log(worldPosition);
                    // Change the material of all hit colliders
                    // to use a transparent shader.
                    int x = Mathf.RoundToInt(worldPosition.x);
                    int y = Mathf.RoundToInt(worldPosition.y);
                    int z = Mathf.RoundToInt(worldPosition.z);
                    _flyingBuilding.transform.position = new Vector3(x,y,z);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                ITile tile = _flyingBuilding.GetComponent<ITile>();
                tile.CheckPos();

                _flyingBuilding = null;
            }
        }
    }

    public void SetBuidingMode()
    {
        IsBuildingMode = !IsBuildingMode;
    }
}

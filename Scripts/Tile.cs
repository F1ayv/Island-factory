using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private int _sizeX, _sizeZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeZ; y++)
            {
                if((x+y)%2==1)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.blue;
                
                Vector3 pos = new Vector3( + x + transform.position.x , 0, + y + transform.position.z );
                Gizmos.DrawCube(pos, new Vector3(1, 0.1f, 1));
            }
        }
    }

    public Vector2 GetSize()
    {
        return new Vector2(_sizeX, _sizeZ);
    }
}

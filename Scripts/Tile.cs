using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile :MonoBehaviour, ITile
{
    [SerializeField]
    protected int _sizeX, _sizeZ;
    [SerializeField]
    public List<string> isCanInstallTag;
    [SerializeField]
    private bool isInstalling = true;
    public int buidingsStay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CheckCanInstall()
    {
        if (isInstalling)
        {
            for (int x = 0; x < _sizeX; x++)
            {
                for (int y = 0; y < _sizeZ; y++)
                {
                    Ray ray = new Ray(transform.position + transform.right * x + transform.forward * y, Vector3.down);
                    RaycastHit[] hits = Physics.RaycastAll(ray, 0.7f);
                    bool isGround = false;
                    foreach (var hit in hits)
                    {
                        if (hit.transform.CompareTag("Ground"))
                        {
                            isGround = true;
                            break;
                        }
                    }
                    if (!isGround)
                    {
                        Debug.Log("Объект не на земле!");
                    }

                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform t = other.transform;
        if(other.CompareTag("Output") || other.CompareTag("Input"))
            return;
        Debug.Log("buildings Stay = " + other.name);
        buidingsStay++;
    }

    private void OnTriggerExit(Collider other)
    {
       if(other.CompareTag("Output") || other.CompareTag("Input"))
            return;
       buidingsStay--;
       Debug.Log("buildings Stay = " + buidingsStay);
    }

    public Vector2 GetSize()
    {
        return new Vector2(_sizeX, _sizeZ);
    }
    public abstract void CheckPos();

    public bool CanInstall()
    {
        return buidingsStay == 0;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up / 3, Vector3.back);

        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeZ; y++)
            {
                if ((x + y) % 2 == 1)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.blue;

                Vector3 pos = new Vector3(+x + transform.position.x, 0, +y + transform.position.z);
                Gizmos.DrawCube(pos, new Vector3(1, 0.1f, 1));
            }
        }
    }
}

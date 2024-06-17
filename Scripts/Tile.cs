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
    public bool isInstalling;
    public int buidingsStay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}

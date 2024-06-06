using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WoodMiner : Mekanism
{
    public override bool GetRes(GameObject res)
    {
        return false;
    }
}

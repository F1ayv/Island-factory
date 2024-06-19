using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WoodMiner : Mekanism
{
    public override bool GetRes(Res res)
    {
        return false;
    }

    public override void SetParamsInstall()
    {
        prefabRes = findedGO.GetComponent<ResGen>().resPrefab;
    }
}

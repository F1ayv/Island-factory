using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperDowner : Mekanism
{
    // Start is called before the first frame update
    public override bool GetRes(GameObject res)
    {
        if (GenerationRes(res))
        {
            Destroy(res);
            return true;
        }
        return false;
    }

}

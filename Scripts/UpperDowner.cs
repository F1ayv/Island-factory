using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperDowner : Mekanism
{
    // Start is called before the first frame update
    public override bool GetRes(Res res)
    {
        if (GenerationRes(res.gameObject))
        {
            Destroy(res.gameObject);
            return true;
        }
        return false;
    }

}

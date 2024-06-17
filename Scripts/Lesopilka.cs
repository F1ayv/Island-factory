using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesopilka : Mekanism
{
    int resTargerID = 0;
    // Start is called before the first frame update
    public override bool GetRes(Res res)
    {
        foreach (var inp in Recipe.inputs)
        {
            if(inp.res.resId != res.resId)
                continue;

            if (ResInside.ContainsKey(res.resId) && ResInside[res.resId] < inp.amount)
            {
                ResInside[res.resId]++;
                Destroy(res.gameObject);
                return true;
            }
        }
        return false;
    }

}

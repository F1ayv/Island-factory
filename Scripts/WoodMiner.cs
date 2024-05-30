using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodMiner : Mekanism
{
    // Start is called before the first frame update
    public override void GenerationRes(GameObject gameObject)
    {
        if (Outputs.Count == 0)
            return;
        
        if(Outputs[0].Input == null)
            return;
        
        GameObject res = Instantiate(gameObject, Outputs[0].Input.GetPos());
        res.transform.position = new Vector3(transform.position.x, 0.4f + res.transform.localScale.y/2, transform.position.z);
        Outputs[0].Input.NewRes(res);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSale : Mekanism
{
    // Start is called before the first frame update
    [SerializeField]
    private CoinManager _coinManager;
    public override bool GetRes(GameObject res)
    {
        Res res1 = res.GetComponent<Res>();
        if (res1 == null)
        {
            Destroy(res);
            return true;
        }

        _coinManager.AddCoins(res1.price);
        Destroy(res);
        return true;

    }
    
}

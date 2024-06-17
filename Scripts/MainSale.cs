using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSale : Mekanism
{
    // Start is called before the first frame update
    [SerializeField]
    private CoinManager _coinManager;
    public override bool GetRes(Res res)
    {
        if (res == null)
        {
            Destroy(res.gameObject);
            return true;
        }

        _coinManager.AddCoins(res.price);
        Destroy(res.gameObject);
        return true;

    }
    
}

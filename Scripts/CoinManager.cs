using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int coinValue = 0;
    public TextMeshProUGUI textCoin;
    // Start is called before the first frame update
    void Start()
    {
        textCoin.text = coinValue.ToString();
    }

    public void AddCoins(int value)
    {
        coinValue += value;
        textCoin.text = coinValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

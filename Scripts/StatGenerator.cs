using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class StatGenerator : MonoBehaviour
{
    public List<TextMeshProUGUI> tmpList;
    public List<int> resId;
    public List<int> sum = new();

    [SerializeField]
    private float _timerSpawnerRes, _reloadTimeSpawnerRes = 10;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var id in resId)
        {
            sum.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timerSpawnerRes += Time.deltaTime;
        if (_timerSpawnerRes >= _reloadTimeSpawnerRes)
        {
            for (int i = 0; i<resId.Count; i++)
            {
                tmpList[i].text = (Mathf.Round(sum[i])*60/_reloadTimeSpawnerRes).ToString() + " ед./мин.";
                sum[i] = 0;
            }
            _timerSpawnerRes = 0;
        }
    }

    public void AddSum(int resID,int count)
    {
        for (int i = 0; i<resId.Count;i++)
        {
            if (resID == resId[i])
            {
                sum[i] += count;
            }
        }
    }
}

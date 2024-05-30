using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mekanism : Tile, IMekanism
{
    public List<IInput> Inputs { get; set;}
    public List<IOutput> Outputs { get; set ; }
    public GameObject prefabRes;
    
    private float _timerSpawnerRes, _reloadTimeSpawnerRes = 1;
    private float lastOutput;
    public abstract void GenerationRes(GameObject gameObject);

    public void FindOutputs()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "Output")
            {
                Output output = transform.GetChild(i).GetComponent<Output>();
                Outputs.Add(output);
            }
        }
    }

    public override void CheckPos()
    {
        foreach (var output in Outputs)
        {
            if(!(output is Output))
                continue;
            
            Output outputReal = output as Output;
            outputReal.CheckPos();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Inputs = new List<IInput>();
        Outputs = new List<IOutput>();
        FindOutputs();
    }

    // Update is called once per frame
    void Update()
    {
        _timerSpawnerRes += Time.deltaTime;
        if (_timerSpawnerRes >= _reloadTimeSpawnerRes)
        {
            GenerationRes(prefabRes);
            _timerSpawnerRes = 0;
        }
    }
}

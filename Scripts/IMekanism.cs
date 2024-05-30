using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMekanism
{
    List<IInput> Inputs { get; set; }
    List<IOutput> Outputs { get; set; }
    public void GenerationRes(GameObject gameObject);
    void FindOutputs();
}

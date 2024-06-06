using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMekanism
{
    List<IInput> Inputs { get; set; }
    List<IOutput> Outputs { get; set; }
    public bool GenerationRes(GameObject gameObject);
    public bool GetRes(GameObject res);
    void FindOutputsInputs();
}

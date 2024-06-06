using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
    IOutput Output { get; set; }
    void NewRes(GameObject res);
    bool CheckOpacity();
    void SetOutput(IInput b);
    Transform GetPos();
    IInput GetOutput();
}

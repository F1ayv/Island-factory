using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
    IOutput Output { get; set; }
    void NewRes(Tuple<int,GameObject> res);
    void SetOutput(IInput b);
    Transform GetPos();
    IInput GetOutput();
}

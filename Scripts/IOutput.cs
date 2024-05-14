using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOutput
{
    IInput Input { get; set; }
    void SetInput(IOutput b);
    Transform GetPos();
    IOutput GetInput();
}

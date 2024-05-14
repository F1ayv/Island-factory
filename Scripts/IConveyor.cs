using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConveyor: IInput, IOutput
{
    void RemoveRes(GameObject res);
    Transform GetPos();
    void SetCheckLastPosRes(bool b);
}

using System.Collections;
using System.Collections.Generic;
using Core.Scripts;
using UnityEngine;

public interface IUpdateable
{
    void DoFirstUpdate();

    void DoUpdate(TimeValues argTime);
}

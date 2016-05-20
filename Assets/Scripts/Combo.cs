using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

public abstract class Combo : MonoBehaviour
{
    public bool isFinished = false;
    public Vector2 lastQTEPosition;
    public VerticalBounds verticalBounds;
    
    public abstract void Execute(GameObject lastQTE, GameObject QTEPrefab);
}


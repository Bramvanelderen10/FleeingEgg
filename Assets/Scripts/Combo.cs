using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Combo
{
    public bool inProgress;
    public int comboLength;
    public int currentLength;
    public float horizontalDistance;

    public QuickTimeEventController lastQte;
    public GameObject qtePrefab;

    public Combo(bool inProgress, int comboLength, int currentLength, float horizontalDistance)
    {
        this.inProgress = inProgress;
        this.comboLength = comboLength;
        this.currentLength = currentLength;
        this.horizontalDistance = horizontalDistance;
    }

    //public generatenextqte()
    //{


    //    lastqte.gameobject.transform.position.x
    //}

}


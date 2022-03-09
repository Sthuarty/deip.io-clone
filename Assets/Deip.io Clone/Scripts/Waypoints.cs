using System.Collections.Generic;
using UnityEngine;

public class Waypoints : Singleton<Waypoints> {
    [HideInInspector] public Transform[] list;


    public override void Awake() {
        base.Awake();

        List<Transform> tempList = new List<Transform>();

        foreach (Transform chill in transform)
            tempList.Add(chill);

        list = tempList.ToArray();
    }
}

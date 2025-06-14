using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPositionManager : Singleton<PatrolPositionManager>
{
    private Dictionary<string, List<Transform>> positionsDict = new Dictionary<string, List<Transform>>();

    public List<Transform> GetPositions(string nameHolder)
    {
        if (!positionsDict.ContainsKey(nameHolder))
        {
            Transform holder = transform.Find(nameHolder);
            if (holder == null)
            {
                Debug.LogWarning("Can not find " + nameHolder + " in patrol position");
                return null;
            }

            List<Transform> handler = new List<Transform>();
            foreach (Transform trf in holder)
            {
                handler.Add(trf);
            }

            positionsDict[nameHolder] = handler;
        }

        return positionsDict[nameHolder];
    }
}

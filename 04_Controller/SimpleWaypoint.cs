using System.Collections.Generic;
using UnityEngine;

namespace Splatoon2
{
    public class SimpleWaypoint : MonoBehaviour
    {
        private int index = 0;
        private List<GameObject> waypoints;





        void Start()
        {
            waypoints = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                waypoints.Add(transform.GetChild(i).gameObject);
            }
        }



        public Vector3 UpdatePos()
        {
            index = (index + 1) % waypoints.Count;
            return waypoints[index].transform.position;
        }
    }
}
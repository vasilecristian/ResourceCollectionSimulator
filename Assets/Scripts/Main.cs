using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using rcs;
using fmp;

public class Main : MonoBehaviour
{
    NavMesh m_map = null;
    FindMyPath m_pathEngine = null;
    List<Barn> m_homes = null;
    List<Worker> m_workers = null;
    List<Mineral> m_minerals = null;

    // Start is called before the first frame update
    void Start()
    {
        m_map = new NavMesh();
        m_pathEngine = new FindMyPath(m_map);

        m_map.Generate3DMap();

        Barn b = new Barn(10, 9, 9);

        Mineral m = new Mineral(8, 8);

        Worker w = new Worker(7, 7);

        //GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = new Vector3(0, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

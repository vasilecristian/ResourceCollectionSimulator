using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using rcs;
using fmp;

public class Main : MonoBehaviour
{
    Playspace m_playspace = null;
    FindMyPath m_pathEngine = null;
    List<Storage> m_homes = null;
    List<Agents> m_workers = null;
    List<Rsources> m_minerals = null;

    // Start is called before the first frame update
    void Start()
    {
        m_playspace = new Playspace();
        m_pathEngine = new FindMyPath(m_playspace);

        m_playspace.Generate3DMap();

        Storage b = new Storage(10, 9, 9);

        Rsources m = new Rsources(8, 8);

        Agents w = new Agents(7, 7);

        //GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = new Vector3(0, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

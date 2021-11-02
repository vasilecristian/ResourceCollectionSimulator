using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using rcs;
using fmp;

public class Main : MonoBehaviour
{ 
    const uint mapWidth = 8;
    const uint mapHeight = 8;
    const uint resourceSpawnLocations = 4;
    const uint numberOfAgents = resourceSpawnLocations;
    const uint storageSpawnLocations = 2;


    System.Random m_rnd = new System.Random();
    Playspace m_playspace = null;
    FindMyPath m_pathEngine = null;
    List<Storage> m_storages = new List<Storage>();
    List<Agent> m_agents = new List<Agent>();
    List<rcs.Resource> m_resources = new List<rcs.Resource>();
    

    // Start is called before the first frame update
    void Start()
    {
        m_playspace = new Playspace();
        m_pathEngine = new FindMyPath(m_playspace);

        Generate3DMap();
        SpawnAgents();
        SpawnResources();
        SpawnStoreges();

        //Storage b = new Storage(10, 9, 9);

        //rcs.Resource m = new rcs.Resource(ResourceType.COOPER, 8, 8);

        //Agent w = new Agent(0, 0, m_resources, m_storages, m_playspace, m_pathEngine, m_rnd);

        

        //GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = new Vector3(0, 0.5f, 0);
    }

    //======================================================================
    void Generate3DMap()
    {
        for (ulong y = 0; y < m_playspace.Height; y++)
        {
            for (ulong x = 0; x < m_playspace.Width; x++)
            {
                //ulong index = y * m_playspace.Width + x;

                long nodeValue = m_playspace.GetNodeValue(x, y);

                if (nodeValue == Playspace.I)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(x * 10, 1, y * 10);
                    cube.transform.localScale = new Vector3(8, 2, 8);
                    cube.GetComponent<Renderer>().material.color = new Color32(177, 199, 201, 1);
                }

                GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plane.transform.position = new Vector3(x * 10, 0, y * 10);
                plane.GetComponent<Renderer>().material.color = new Color32(60, 84, 86, 1);

            }
        }
    }

    //======================================================================
    void SpawnResources()
    {
        for (ulong y = 0; y < m_playspace.Height; y++)
        {
            for (ulong x = 0; x < m_playspace.Width; x++)
            {
                //ulong index = y * m_playspace.Width + x;

                long nodeValue = m_playspace.GetNodeValue(x, y);

                if (nodeValue == Playspace.R)
                {
                    ResourceType type = (ResourceType)m_rnd.Next(3);
                    m_resources.Add(new rcs.Resource(new PositionOnPlayspace(x, y), type));
                }
            }
        }
    }


    //======================================================================
    void SpawnStoreges()
    {
        for (ulong y = 0; y < m_playspace.Height; y++)
        {
            for (ulong x = 0; x < m_playspace.Width; x++)
            {
                //ulong index = y * m_playspace.Width + x;

                long nodeValue = m_playspace.GetNodeValue(x, y);

                if (nodeValue == Playspace.S)
                {
                    m_storages.Add(new Storage(new PositionOnPlayspace(x, y), 10));
                }
            }
        }
    }


    //======================================================================
    void SpawnAgents()
    { 
        do
        {
            ulong x = 0;
            ulong y = 0;
            do
            {
                x = ((ulong)m_rnd.Next(((int)m_playspace.Width)));
                y = ((ulong)m_rnd.Next(((int)m_playspace.Width)));
            }
            while (m_playspace.GetNodeValue(x, y) != Playspace.O);

            m_agents.Add(new Agent(new PositionOnPlayspace(x, y), m_resources, m_storages, m_playspace, m_pathEngine, m_rnd));
        }
        while (m_agents.Count < numberOfAgents);
    }


    // Update is called once per frame
    void Update()
    {
        foreach(Agent agent in m_agents)
        {
            agent.Update();
        }

       
    }
}

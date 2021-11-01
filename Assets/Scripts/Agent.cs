using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

using fmp;

namespace rcs
{
    public enum AgentState
    {
        IDLE,
        SEARCHING_RESOURCES,
        COMPUTE_PATH,
        GOTO_RESOURCE,
        SEARCHING_STORAGE,
        GOTO_STORAGE,
        WAITING_FOR_STORAGE,
    };

    public class Agent
    {
        System.Random m_rnd = null;

        GameObject m_object3d = null;

        ulong m_mapPosX = 0;
        ulong m_mapPosY = 0;
        float m_speed = 8;

        List<rcs.Resource> m_resourcesList;
        List<Storage> m_storagesList;
        Playspace m_playspace;
        FindMyPath m_pathEngine;

        AgentState m_agentState = AgentState.SEARCHING_RESOURCES;
        rcs.Resource m_targetResource = null;
        rcs.Storage m_targetStorage = null;
        List<ulong> m_pathToTarget = null;
        int m_pathToTargetNextNodeIndex = 0;


        public Agent(
            ulong mapPosX,
            ulong mapPosY,
            List<rcs.Resource> resourcesList,
            List<Storage> storagesList,
            Playspace playspace,
            FindMyPath pathEngine,
            System.Random rnd)
        {
            m_mapPosX = mapPosX;
            m_mapPosY = mapPosY;
            m_resourcesList = resourcesList;
            m_storagesList = storagesList;
            m_playspace = playspace;
            m_pathEngine = pathEngine;
            m_rnd = rnd;
            

            m_object3d = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            m_object3d.transform.position = new Vector3(m_mapPosX * 10, 2.5f, m_mapPosY * 10);
            m_object3d.transform.localScale = new Vector3(3, 3, 3);
            m_object3d.GetComponent<Renderer>().material.color = new Color32(30, 206, 118, 1);
        }

        public ulong MapPosX
        {
            get { return m_mapPosX; }
        }

        public ulong MapPosY
        {
            get { return m_mapPosY; }
        }

        public void Update()
        {
            if(m_agentState == AgentState.IDLE)
            {
                //m_agentState = AgentState.SEARCHING_RESOURCES;

            }
            else if(m_agentState == AgentState.SEARCHING_RESOURCES)
            {
                int resourceChoosed = m_rnd.Next(m_resourcesList.Count);
                m_targetResource = m_resourcesList[resourceChoosed];

                Ticket ticket = new Ticket(m_playspace.GetIndex(m_mapPosX, m_mapPosY), m_playspace.GetIndex(m_targetResource.MapPosX, m_targetResource.MapPosY));

                CancellationTokenSource cancelToken = new CancellationTokenSource();
                Task<Ticket> task = m_pathEngine.FindPathAsync(ticket, cancelToken);
                task.ContinueWith(previousTask =>
                {
                    if (previousTask.Result.State == Ticket.STATE.COMPLETED)
                    {
                        m_pathToTarget = previousTask.Result.Path;
                        m_pathToTarget.Reverse();
                        m_pathToTargetNextNodeIndex = 0;
                        m_agentState = AgentState.GOTO_RESOURCE;
                    }
                    else
                    {
                        m_agentState = AgentState.SEARCHING_RESOURCES;
                    }
                });
                m_agentState = AgentState.COMPUTE_PATH;

            }
            else if (m_agentState == AgentState.GOTO_RESOURCE)
            {
                MapPos mapPos = m_playspace.GetMapPos(m_pathToTarget[m_pathToTargetNextNodeIndex]);
                Vector3 intermediateTarget = new Vector3(mapPos.x * 10, 2.5f, mapPos.y * 10);

                if (Vector3.Distance(intermediateTarget, m_object3d.transform.position) > 0.2)
                {
                    Vector3 direction = Vector3.Normalize(new Vector3(mapPos.x * 10, 2.5f, mapPos.y * 10) - m_object3d.transform.position);
                    m_object3d.transform.Translate(direction * m_speed * Time.deltaTime);
                }
                else
                {
                    m_mapPosX = mapPos.x;
                    m_mapPosY = mapPos.y;
                    m_pathToTargetNextNodeIndex++;
                }

                if(m_pathToTargetNextNodeIndex == m_pathToTarget.Count)
                {
                    /// TARGET REACHED
                    m_pathToTarget = null;



                    m_agentState = AgentState.SEARCHING_STORAGE;
                }

            }
            else if (m_agentState == AgentState.SEARCHING_STORAGE)
            {
                int storageChoosed = m_rnd.Next(m_storagesList.Count);
                m_targetStorage = m_storagesList[storageChoosed];

                Ticket ticket = new Ticket(m_playspace.GetIndex(m_mapPosX, m_mapPosY), m_playspace.GetIndex(m_targetStorage.MapPosX, m_targetStorage.MapPosY));

                CancellationTokenSource cancelToken = new CancellationTokenSource();
                Task<Ticket> task = m_pathEngine.FindPathAsync(ticket, cancelToken);
                task.ContinueWith(previousTask =>
                {
                    if (previousTask.Result.State == Ticket.STATE.COMPLETED)
                    {
                        m_pathToTarget = previousTask.Result.Path;
                        m_pathToTarget.Reverse();
                        m_pathToTargetNextNodeIndex = 0;
                        m_agentState = AgentState.GOTO_STORAGE;
                    }
                    else
                    {
                        m_agentState = AgentState.SEARCHING_STORAGE;
                    }
                });
                m_agentState = AgentState.COMPUTE_PATH;

            }
            else if (m_agentState == AgentState.GOTO_STORAGE)
            {
                MapPos mapPos = m_playspace.GetMapPos(m_pathToTarget[m_pathToTargetNextNodeIndex]);
                Vector3 intermediateTarget = new Vector3(mapPos.x * 10, 2.5f, mapPos.y * 10);

                if (Vector3.Distance(intermediateTarget, m_object3d.transform.position) > 0.2)
                {
                    Vector3 direction = Vector3.Normalize(new Vector3(mapPos.x * 10, 2.5f, mapPos.y * 10) - m_object3d.transform.position);
                    m_object3d.transform.Translate(direction * m_speed * Time.deltaTime);
                }
                else
                {
                    m_mapPosX = mapPos.x;
                    m_mapPosY = mapPos.y;
                    m_pathToTargetNextNodeIndex++;
                }

                if (m_pathToTargetNextNodeIndex == m_pathToTarget.Count)
                {
                    /// TARGET REACHED
                    m_pathToTarget = null;



                    m_agentState = AgentState.IDLE;
                }

            }
        }
        
    }

    
}

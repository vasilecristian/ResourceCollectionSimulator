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
        CHOOSE_RESOURCE,
        COMPUTE_PATH,
        GOTO_RESOURCE,
        CHOOSE_STORAGE,
        GOTO_STORAGE,
        WAITING_FOR_STORAGE,
    };

    public class Agent : PlayspaceObject
    {
        System.Random m_rnd = null;

        List<rcs.Resource> m_resourcesList;
        List<Storage> m_storagesList;
        Playspace m_playspace;
        FindMyPath m_pathEngine;

        AgentState m_agentState = AgentState.CHOOSE_RESOURCE;
        PlayspaceObject m_target = null;
        rcs.Resource m_payload = null;
        //rcs.Storage m_targetStorage = null;
        List<ulong> m_pathToTarget = null;
        int m_pathToTargetNextNodeIndex = 0;


        public Agent(
            PositionOnPlayspace pos,
            List<rcs.Resource> resourcesList,
            List<Storage> storagesList,
            Playspace playspace,
            FindMyPath pathEngine,
            System.Random rnd):base(pos, 16)
        {
            m_resourcesList = resourcesList;
            m_storagesList = storagesList;
            m_playspace = playspace;
            m_pathEngine = pathEngine;
            m_rnd = rnd;


            Object3D = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            Object3D.transform.position = new Vector3(PositionX * 10, 2.5f, PositionY * 10);
            Object3D.transform.localScale = new Vector3(3, 3, 3);
            Object3D.GetComponent<Renderer>().material.color = new Color32(30, 206, 118, 1);
        }


        public void Update()
        {
            if(m_agentState == AgentState.IDLE)
            {
                //m_agentState = AgentState.SEARCHING_RESOURCES;

            }
            else if(m_agentState == AgentState.CHOOSE_RESOURCE)
            {
                int resourceChoosed = m_rnd.Next(m_resourcesList.Count);
                m_target = m_resourcesList[resourceChoosed];

                Ticket ticket = new Ticket(m_playspace.GetIndex(PositionX, PositionY), m_playspace.GetIndex(m_target.PositionX, m_target.PositionY));

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
                        m_agentState = AgentState.CHOOSE_RESOURCE;
                    }
                });
                m_agentState = AgentState.COMPUTE_PATH;

            }
            else if (m_agentState == AgentState.GOTO_RESOURCE)
            {
                PositionOnPlayspace intermediatePos = m_playspace.GetMapPos(m_pathToTarget[m_pathToTargetNextNodeIndex]);
                Vector3 intermediate3DPos = new Vector3(intermediatePos.x * 10, 2.5f, intermediatePos.y * 10);

                if (Vector3.Distance(intermediate3DPos, Object3D.transform.position) > 0.2)
                {
                    Vector3 direction = Vector3.Normalize(new Vector3(intermediatePos.x * 10, 2.5f, intermediatePos.y * 10) - Object3D.transform.position);
                    Object3D.transform.Translate(direction * Speed * Time.deltaTime);
                }
                else
                {
                    Position = intermediatePos;
                    m_pathToTargetNextNodeIndex++;
                }

                if(m_pathToTargetNextNodeIndex == m_pathToTarget.Count)
                {
                    /// TARGET REACHED
                    m_pathToTarget = null;

                    m_payload = new rcs.Resource(new PositionOnPlayspace(0, 0), ((rcs.Resource)m_target).ResType);

                    m_agentState = AgentState.CHOOSE_STORAGE;
                }

            }
            else if (m_agentState == AgentState.CHOOSE_STORAGE)
            {
                int storageChoosed = m_rnd.Next(m_storagesList.Count);
                m_target = m_storagesList[storageChoosed];

                if (((rcs.Storage)m_target).IsFull)
                {
                    m_agentState = AgentState.CHOOSE_STORAGE;
                }
                else
                {

                    Ticket ticket = new Ticket(m_playspace.GetIndex(PositionX, PositionY), m_playspace.GetIndex(m_target.PositionX, m_target.PositionY));

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
                            m_agentState = AgentState.CHOOSE_STORAGE;
                        }
                    });
                    m_agentState = AgentState.COMPUTE_PATH;
                }

            }
            else if (m_agentState == AgentState.GOTO_STORAGE)
            {
                PositionOnPlayspace intermediatePos = m_playspace.GetMapPos(m_pathToTarget[m_pathToTargetNextNodeIndex]);
                Vector3 intermediate3DPos = new Vector3(intermediatePos.x * 10, 2.5f, intermediatePos.y * 10);

                if (Vector3.Distance(intermediate3DPos, Object3D.transform.position) > 0.2)
                {
                    Vector3 direction = Vector3.Normalize(new Vector3(intermediatePos.x * 10, 2.5f, intermediatePos.y * 10) - Object3D.transform.position);
                    Object3D.transform.Translate(direction * Speed * Time.deltaTime);
                }
                else
                {
                    Position = intermediatePos;
                    m_pathToTargetNextNodeIndex++;
                }

                if (m_pathToTargetNextNodeIndex == m_pathToTarget.Count)
                {
                    /// TARGET REACHED
                    m_pathToTarget = null;

                    if (((rcs.Storage)m_target).IsFull)
                    {
                        m_agentState = AgentState.CHOOSE_STORAGE;
                    }

                    if (((rcs.Storage)m_target).Add(m_payload))
                    {
                        m_payload = null;
                        m_agentState = AgentState.CHOOSE_RESOURCE;
                    }
                    else
                    {
                        m_agentState = AgentState.CHOOSE_STORAGE;
                    }

                    //m_agentState = AgentState.IDLE;
                }

            }




            if(m_payload != null)
            {
                m_payload.Object3D.transform.position = new Vector3(Object3D.transform.position.x, Object3D.transform.position.y + 3, Object3D.transform.position.z);
            }

        }
        
    }

    
}

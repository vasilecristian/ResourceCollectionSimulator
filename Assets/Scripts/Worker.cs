using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rcs
{
    public class Worker
    {
        GameObject m_object3d = null;

        public Worker(int meshPosX, int meshPosY)
        {
            m_object3d = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            m_object3d.transform.position = new Vector3(meshPosX * 10, 2.5f, meshPosY * 10);
            m_object3d.transform.localScale = new Vector3(3, 3, 3);
            m_object3d.GetComponent<Renderer>().material.color = new Color32(30, 206, 118, 1);
        }
        
    }
}

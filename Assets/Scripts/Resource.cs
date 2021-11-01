using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rcs
{
    public enum ResourceType
    {
        GOLD,
        IRON,
        COOPER
    }
    public class Resource
    {
        ulong m_mapPosX = 0;
        ulong m_mapPosY = 0;

        GameObject m_object3d = null;
        ResourceType m_resourceType = ResourceType.COOPER;

        public Resource(ResourceType resType, ulong mapPosX, ulong mapPosY)
        {
            m_resourceType = resType;
            m_mapPosX = mapPosX;
            m_mapPosY = mapPosY;

            m_object3d = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m_object3d.transform.position = new Vector3(m_mapPosX * 10, 0.5f, m_mapPosY * 10);
            m_object3d.transform.localScale = new Vector3(2, 2, 2);

            if (m_resourceType == ResourceType.GOLD)
            {
                m_object3d.GetComponent<Renderer>().material.color = new Color32(242, 238, 16, 1);
            }
            else if (m_resourceType == ResourceType.IRON)
            {
                m_object3d.GetComponent<Renderer>().material.color = new Color32(168, 193, 190, 1);
            }
            else //ResourceType.COOPER
            {
                m_object3d.GetComponent<Renderer>().material.color = new Color32(209, 139, 10, 1);
            }
        }

        public ulong MapPosX
        {
            get { return m_mapPosX; }
        }

        public ulong MapPosY
        {
            get { return m_mapPosY; }
        }
    }

    


}

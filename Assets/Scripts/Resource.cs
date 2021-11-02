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

    public class Resource : PlayspaceObject
    {
        
        ResourceType m_resourceType = ResourceType.COOPER;

        public Resource(PositionOnPlayspace pos, ResourceType resType) :base(pos, 0)
        {
            m_resourceType = resType;

            Object3D = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Object3D.transform.position = new Vector3(PositionX * 10, 0.5f, PositionY * 10);
            Object3D.transform.localScale = new Vector3(2, 2, 2);

            if (m_resourceType == ResourceType.GOLD)
            {
                Object3D.GetComponent<Renderer>().material.color = new Color32(242, 238, 16, 1);
            }
            else if (m_resourceType == ResourceType.IRON)
            {
                Object3D.GetComponent<Renderer>().material.color = new Color32(168, 193, 190, 1);
            }
            else //ResourceType.COOPER
            {
                Object3D.GetComponent<Renderer>().material.color = new Color32(209, 139, 10, 1);
            }
        }

        public ResourceType ResType
        {
            get { return m_resourceType; }
        }
    }

    


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rcs
{
    public class Rsources
    {
        GameObject m_object3d = null;

        public Rsources(int meshPosX, int meshPosY)
        {
            m_object3d = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m_object3d.transform.position = new Vector3(meshPosX * 10, 0.5f, meshPosY * 10);
            m_object3d.transform.localScale = new Vector3(2, 2, 2);
            m_object3d.GetComponent<Renderer>().material.color = new Color32(221, 234, 100, 1);
        }
    }
}

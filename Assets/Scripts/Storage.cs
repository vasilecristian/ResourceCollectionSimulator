using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rcs
{
    public class Storage : PlayspaceObject
    {
        uint m_maxCapatity = 0;
        //uint m_storredItems = 0;
        List<rcs.Resource> m_resourcesStored = new List<rcs.Resource>();

        public Storage(PositionOnPlayspace pos, uint maxCapatity) :base(pos, 0)
        {
            m_maxCapatity = maxCapatity;

            Object3D = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            Object3D.transform.position = new Vector3(PositionX * 10, 0.5f, PositionY * 10);
            Object3D.transform.localScale = new Vector3(3, 3, 3);
            Object3D.GetComponent<Renderer>().material.color = new Color32(109, 100, 234, 1);
        }

  

        /// <summary>
        /// Increment with 1 the storred items. 
        /// </summary>
        /// <returns>True if the object was added.</returns>
        public bool Add(rcs.Resource res)
        {
            if (IsFull)
            {
                return false;
            }

            m_resourcesStored.Add(res);

            res.Object3D.transform.position = new Vector3(Object3D.transform.position.x, Object3D.transform.position.y + 3 * m_resourcesStored.Count, Object3D.transform.position.z);
            
            return true;
        }

        public bool IsFull
        {
            get { return m_maxCapatity <= m_resourcesStored.Count; }
        }

        public uint Count
        {
            get { return ((uint)m_resourcesStored.Count); }
        }

        public uint MaxCapatity
        {
            get { return m_maxCapatity; }
        }
    }
}

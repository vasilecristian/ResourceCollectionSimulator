using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rcs
{
    public class Barn
    {
        uint m_maxCapatity = 0;
        uint m_storredItems = 0;
        GameObject m_object3d = null;

        public Barn(uint maxCapatity, int meshPosX, int meshPosY)
        {
            m_maxCapatity = maxCapatity;

            m_object3d = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            m_object3d.transform.position = new Vector3(meshPosX * 10, 0.5f, meshPosY * 10);
            m_object3d.transform.localScale = new Vector3(3, 3, 3);
            m_object3d.GetComponent<Renderer>().material.color = new Color32(109, 100, 234, 1);
        }

        /// <summary>
        /// Increment with 1 the storred items. 
        /// </summary>
        /// <returns>True if the object was added.</returns>
        public bool Add()
        {
            if (IsFull)
            {
                return false;
            }

            m_storredItems++;
            return true;
        }

        public bool IsFull
        {
            get { return m_maxCapatity <= m_storredItems; }
        }

        public uint Count
        {
            get { return m_storredItems; }
        }

        public uint MaxCapatity
        {
            get { return m_maxCapatity; }
        }
    }
}

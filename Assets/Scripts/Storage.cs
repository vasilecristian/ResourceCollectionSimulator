using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rcs
{
    public class Storage
    {
        ulong m_mapPosX = 0;
        ulong m_mapPosY = 0;
        uint m_maxCapatity = 0;
        uint m_storredItems = 0;
        GameObject m_object3d = null;

        public Storage(uint maxCapatity, ulong mapPosX, ulong mapPosY)
        {
            m_maxCapatity = maxCapatity;
            m_mapPosX = mapPosX;
            m_mapPosY = mapPosY;

            m_object3d = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            m_object3d.transform.position = new Vector3(mapPosX * 10, 0.5f, mapPosY * 10);
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

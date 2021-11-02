using System;
using UnityEngine;

namespace rcs
{
    public class PlayspaceObject
    {
        GameObject m_object3d = null;
        PositionOnPlayspace m_pos = null;
        float m_speed = 0;

        public PlayspaceObject(PositionOnPlayspace pos, float speed)
        {
            m_pos = pos;
            m_speed = speed;
        }

        public GameObject Object3D
        {
            get { return m_object3d; }
            set { m_object3d = value; }
        }

        public PositionOnPlayspace Position
        {
            get { return m_pos; }
            set { m_pos = value; }
        }

        public ulong PositionX
        {
            get { return m_pos.x; }
        }

        public ulong PositionY
        {
            get { return m_pos.y; }
        }

        public float Speed
        {
            get { return m_speed; }
        }
    }
}

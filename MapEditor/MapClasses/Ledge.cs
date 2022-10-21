using Microsoft.Xna.Framework;
using System;

namespace MapEditor.MapClasses
{
    [Serializable]
    class Ledge
    {
        Vector2[] nodes = new Vector2[16];
        private int totalNodes = 0;

        public int TotalNodes
        {
            get { return totalNodes; }
            set { totalNodes = value; }
        }
        private int flags = 0;

        public int Flags
        {
            get { return flags; }
            set { flags = value; }
        }
        public Vector2[] Nodes
        {
            get { return nodes; }
        }
    }
}

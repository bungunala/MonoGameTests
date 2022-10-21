using Microsoft.Xna.Framework;
using System;

namespace MapEditor.MapClasses
{
    [Serializable]
    class MapSegment
    {
        public Vector2 Location;
        int segmentIndex;
        public int Index
        {
            get { return segmentIndex; }
            set { segmentIndex = value; }
        }
    }
}

using Microsoft.Xna.Framework;
using System;

namespace MapEditor.MapClasses
{
    [Serializable]
    class SegmentDefinition
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int sourceIndex;
        public int SourceIndex
        {
            get { return sourceIndex; }
            set { sourceIndex = value; }
        }
        private Rectangle srcRect;
        public Rectangle SourceRectangle
        {
            get { return srcRect; }
            set { srcRect = value; }
        }
        private int flags;
        public int Flags
        {
            get { return flags; }
            set { flags = value; }
        }
        public SegmentDefinition(string _name, int _sourceIndex, Rectangle _srcRect, int _flags)
        {
            Name = _name;
            SourceIndex = sourceIndex;
            SourceRectangle = _srcRect;
            Flags = _flags;
        }

    }
}

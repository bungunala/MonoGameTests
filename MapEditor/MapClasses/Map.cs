using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;
using Microsoft.Xna.Framework;

namespace MapEditor.MapClasses
{
    class Map
    {
        SegmentDefinition[] segmentDefinitions;//los distintos elementos q puedo dibujar (paleta)
        MapSegment[,] mapSegments; //
        int[,] col; //grid de colisiones

        public String[] Scripts = new String[128]; //agreagdo 2011-01-20 para soportar scripts

        Ledge[] ledges;
        private string path = "map";

        //private Text text;

        #region Getters y Setters

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public Ledge[] Ledges
        {
            get { return ledges; }
            set { ledges = value; }
        }

        public int[,] Grid
        {
            get { return col; }
            set { col = value; }
        }

        public SegmentDefinition[] SegmentDefinitions
        {
            get { return segmentDefinitions; }
        }

        public MapSegment[,] Segments
        {
            get { return mapSegments; }
        }

        #endregion

        public Map()
        {
            segmentDefinitions = new SegmentDefinition[512];
            mapSegments = new MapSegment[3, 64];
            col = new int[20, 20];

            ledges = new Ledge[16];
            for (int i = 0; i < 16; i++)
                ledges[i] = new Ledge();
            for (int i = 0; i < 128; i++)
                Scripts[i] = string.Empty;

            ReadSegmentDefinitions();
        }

        private void ReadSegmentDefinitions()
        {
            StreamReader s = new StreamReader(@"Content/maps.zdx");
            string t = "";
            int n;
            int currentTex = 0;
            int curDef = -1;
            Rectangle tRect = new Rectangle();
            string[] split;
            t = s.ReadLine();
            while (!s.EndOfStream)
            {
                t = s.ReadLine();
                if (t.StartsWith("#"))
                {
                    if (t.StartsWith("#src"))
                    {
                        split = t.Split(' ');
                        if (split.Length > 1)
                        {
                            n = Convert.ToInt32(split[1]);
                            currentTex = n - 1;
                        }
                    }
                }
                else
                {
                    curDef++;
                    string name = t;

                    t = s.ReadLine();
                    split = t.Split(' ');
                    if (split.Length > 3)
                    {
                        tRect.X = Convert.ToInt32(split[0]);
                        tRect.Y = Convert.ToInt32(split[1]);
                        tRect.Width =
                            Convert.ToInt32(split[2]) - tRect.X;
                        tRect.Height =
                            Convert.ToInt32(split[3]) - tRect.Y;
                    }
                    else
                        Console.WriteLine("read fail: " + name);
                    int tex = currentTex;
                    t = s.ReadLine();
                    int flags = Convert.ToInt32(t);
                    segmentDefinitions[curDef] = new
                        SegmentDefinition(name, tex, tRect, flags);
                }
            }

        }

        public int AddSegment(int layer, int index)
        {
            for (int i = 0; i < 64; i++)
            {
                if (mapSegments[layer, i] == null)
                {
                    mapSegments[layer, i] = new MapSegment();
                    mapSegments[layer, i].Index = index;
                    return i;
                }
            }
            return -1;
        }

        public void Draw(SpriteBatch sprite, Texture2D[] mapsTex, Vector2 scroll)
        {
            Rectangle srcRectangle = new Rectangle();
            Rectangle destRectangle = new Rectangle();
            sprite.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            for (int l = 0; l < 3; l++)
            {
                float scale = 1.0f;
                Color color = Color.White;
                if (l == 0)
                {
                    color = Color.Gray;
                    scale = 0.75f;
                }
                else if (l == 2)
                {
                    color = Color.DarkGray;
                    scale = 1.25f;
                }
                scale *= 0.5f;
                for (int i = 0; i < 64; i++)
                {
                    if (mapSegments[l, i] != null)
                    {
                        srcRectangle = segmentDefinitions[mapSegments[l, i].Index].SourceRectangle;
                        destRectangle.X = (int)(mapSegments[l, i].Location.X - scroll.X * scale);
                        destRectangle.Y = (int)(mapSegments[l, i].Location.Y - scroll.Y * scale);
                        destRectangle.Width = (int)(srcRectangle.Width * scale);
                        destRectangle.Height = (int)(srcRectangle.Height * scale);
                        sprite.Draw(mapsTex[segmentDefinitions[mapSegments[l, i].Index].SourceIndex], destRectangle, srcRectangle, color);
                    }
                }
            }
            sprite.End();
        }

        public int GetHoveredSegment(int x, int y, int layer, Vector2 scroll) //l es de layer, osea la capa en la q estamos
        {
            float scale = 1.0f;
            if (layer == 0)
                scale = 0.75f;
            else if (layer == 2)
                scale = 1.25f;

            scale *= 0.5f;

            for (int i = 63; i >= 0; i--)
            {
                if (mapSegments[layer, i] != null)
                {
                    Rectangle sRect = segmentDefinitions[mapSegments[layer, i].Index].SourceRectangle;
                    Rectangle dRect = new Rectangle(
                        (int)(mapSegments[layer, i].Location.X - scroll.X * scale),
                        (int)(mapSegments[layer, i].Location.Y - scroll.Y * scale),
                        (int)(sRect.Width * scale),
                        (int)(sRect.Height * scale)
                        );

                    if (dRect.Contains(x, y))
                        return i;
                }
            }

            return -1;
        }

        public void Write()
        {
            try
            {
                BinaryWriter file = new BinaryWriter(File.Open(@"Content/data/" + Path + ".zmx", FileMode.Create));

                for (int i = 0; i < ledges.Length; i++)
                {
                    file.Write(ledges[i].TotalNodes);
                    for (int n = 0; n < ledges[i].TotalNodes; n++)
                    {
                        file.Write(ledges[i].Nodes[n].X);
                        file.Write(ledges[i].Nodes[n].Y);
                    }
                    file.Write(ledges[i].Flags);
                }

                for (int l = 0; l < 3; l++)
                {
                    for (int i = 0; i < 64; i++)
                    {
                        if (mapSegments[l, i] == null)
                            file.Write(-1);
                        else
                        {
                            file.Write(mapSegments[l, i].Index);
                            file.Write(mapSegments[l, i].Location.X);
                            file.Write(mapSegments[l, i].Location.Y);
                        }
                    }
                }

                for (int x = 0; x < 20; x++)
                {
                    for (int y = 0; y < 20; y++)
                    {
                        file.Write(col[x, y]);
                    }
                }

                for (int i = 0; i < Scripts.Length; i++)
                    file.Write(Scripts[i]);

                file.Close();
            }
            catch (Exception ex)
            {
                //text.Color = Color.Red;
                //text.DrawText(0, 0, "Error al Guardar el archivo");
            }
        }

        public void Read()
        {
            try
            {
                BinaryReader file = new BinaryReader(File.Open(@"Content/data/" + Path + ".zmx", FileMode.Open));

                for (int i = 0; i < ledges.Length; i++)
                {
                    ledges[i] = new Ledge();
                    ledges[i].TotalNodes = file.ReadInt32();
                    for (int n = 0; n < ledges[i].TotalNodes; n++)
                    {
                        ledges[i].Nodes[n] = new Vector2(file.ReadSingle(), file.ReadSingle());
                    }
                    ledges[i].Flags = file.ReadInt32();
                }

                for (int l = 0; l < 3; l++)
                {
                    for (int i = 0; i < 64; i++)
                    {
                        int t = file.ReadInt32();

                        if (t == -1)
                            mapSegments[l, i] = null;
                        else
                        {
                            mapSegments[l, i] = new MapSegment();
                            mapSegments[l, i].Index = t;
                            mapSegments[l, i].Location = new Vector2(file.ReadSingle(), file.ReadSingle());
                        }
                    }
                }

                for (int x = 0; x < 20; x++)
                {
                    for (int y = 0; y < 20; y++)
                    {
                        col[x, y] = file.ReadInt32();
                    }
                }

                for (int i = 0; i < Scripts.Length; i++)
                    Scripts[i] = file.ReadString();

                file.Close();
            }
            catch
            {
                //text.Color = Color.Red;
                //text.DrawText(0, 0, "Error al Cargar el archivo");
            }
        }
    }
}

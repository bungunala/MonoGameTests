using MapEditor.MapClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TextLib;

namespace MapEditor
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        Text text;
        SpriteFont font;
        Texture2D[] mapsTex;
        Texture2D nullTex;
        Texture2D iconsTex;
        int mouseX, mouseY;
        bool rightMouseDown;
        bool midMouseDown;
        bool mouseClick;
        int mouseDragSeg = -1;
        int curLayer = 1;
        int prevMouseX, prevMouseY;

        MouseState mouseState;
        MouseState preState;

        //Scroll para los elementos de la paleta
        int segScroll;
        int scriptScroll;
        int selScript = -1;

        const int COLOR_NONE = 0;
        const int COLOR_YELLOW = 1;
        const int COLOR_GREEN = 2;


        Vector2 scroll;
        DrawingMode drawType = DrawingMode.SegmentSelection;

        KeyboardState oldKeyState;
        EditingMode editMode = EditingMode.None;

        int curLedge = 0;
        //int curNode=0;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        private bool InitGraphicsMode(int iWidth, int iHeight, bool bFullScreen)
        {
            // If we aren't using a full screen mode, the height and width of the window can
            // be set to anything equal to or smaller than the actual screen size.
            if (bFullScreen == false)
            {
                if ((iWidth <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    && (iHeight <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    graphics.PreferredBackBufferWidth = iWidth;
                    graphics.PreferredBackBufferHeight = iHeight;
                    graphics.IsFullScreen = bFullScreen;
                    graphics.ApplyChanges();
                    return true;
                }
            }
            else
            {
                // If we are using full screen mode, we should check to make sure that the display
                // adapter can handle the video mode we are trying to set.  To do this, we will
                // iterate thorugh the display modes supported by the adapter and check them against
                // the mode we want to set.
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    // Check the width and height of each mode against the passed values
                    if ((dm.Width == iWidth) && (dm.Height == iHeight))
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        graphics.PreferredBackBufferWidth = iWidth;
                        graphics.PreferredBackBufferHeight = iHeight;
                        graphics.IsFullScreen = bFullScreen;
                        graphics.ApplyChanges();
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            map = new Map();
            this.InitGraphicsMode(1280, 720, false);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>(@"Fonts/Arial");
            text = new Text(spriteBatch, font);
            nullTex = Content.Load<Texture2D>(@"gfx/1x1");
            iconsTex = Content.Load<Texture2D>(@"gfx/icons");
            mapsTex = new Texture2D[1];
            for (int i = 0; i < mapsTex.Length; i++)
                mapsTex[i] = Content.Load<Texture2D>(@"gfx/maps" +
                        (i + 1).ToString());

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            // TODO: Add your update logic here

            UpdateKeys();

            mouseState = Mouse.GetState();
            mouseX = mouseState.X;
            mouseY = mouseState.Y;
            bool pMouseDown = rightMouseDown;
            midMouseDown = (mouseState.MiddleButton == ButtonState.Pressed);
            if (midMouseDown)
            {
                scroll.X -= (mouseX - prevMouseX) * 2.0f;
                scroll.Y -= (mouseY - prevMouseY) * 2.0f;
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (!rightMouseDown && GetCanEdit())
                {
                    if (drawType == DrawingMode.SegmentSelection)
                    {
                        int f = map.GetHoveredSegment(mouseX, mouseY, curLayer, scroll);
                        if (f != -1)
                            mouseDragSeg = f;
                    }
                    else if (drawType == DrawingMode.CollitionMap)
                    {
                        int x = (mouseX + (int)(scroll.X / 2)) / 32;
                        int y = (mouseY + (int)(scroll.Y / 2)) / 32;
                        if (x >= 0 && y >= 0 && x < 20 && y < 20)
                        {
                            if (mouseState.LeftButton == ButtonState.Pressed)
                                map.Grid[x, y] = 1;
                            else if (mouseState.RightButton == ButtonState.Pressed)
                                map.Grid[x, y] = 0;
                        }
                    }
                    else if (drawType == DrawingMode.Ledge)
                    {
                        if (map.Ledges[curLedge] == null)
                            map.Ledges[curLedge] = new Ledge();

                        if (map.Ledges[curLedge].TotalNodes < 15)
                        {
                            map.Ledges[curLedge].Nodes[map.Ledges[curLedge].TotalNodes] = new Vector2(mouseX, mouseY) + scroll / 2f;
                            map.Ledges[curLedge].TotalNodes++;
                        }
                    }
                    else if (drawType == DrawingMode.Script)
                    {
                        if (selScript > -1)
                        {
                            if (mouseX < 400)
                                map.Scripts[selScript] += (" " +
                                    ((int)((float)mouseX + scroll.X / 2f)).ToString() + " " +
                                    ((int)((float)mouseY + scroll.Y / 2f)).ToString());
                        }
                    }
                }
                rightMouseDown = true;
            }
            else
                rightMouseDown = false;


            if (pMouseDown && !rightMouseDown) mouseClick = true;
            if (mouseDragSeg > -1)
            {
                if (!rightMouseDown)
                    mouseDragSeg = -1;
                else
                {
                    Vector2 loc = map.Segments[curLayer, mouseDragSeg].Location;
                    loc.X += (mouseX - prevMouseX);
                    loc.Y += (mouseY - prevMouseY);
                    map.Segments[curLayer, mouseDragSeg].Location = loc;
                }
            }
            prevMouseX = mouseX;
            prevMouseY = mouseY;
            preState = mouseState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            this.map.Draw(spriteBatch, mapsTex, scroll);
            //bool drawRest = true;
            switch (drawType)
            {
                case DrawingMode.SegmentSelection:
                    DrawMapSegments();
                    break;
                case DrawingMode.CollitionMap:
                    //DrawMapSegments();
                    break;
                case DrawingMode.Ledge:
                    this.DrawLedgePalette();
                    break;
                case DrawingMode.Script:
                    this.DrawScript();
                    //drawRest = false;
                    break;
            }
            if (DrawButton(5, 65, 3, mouseX, mouseY, mouseClick))
                map.Write();
            if (DrawButton(40, 65, 4, mouseX, mouseY, mouseClick))
                map.Read();

            DrawLedges();
            DrawGrid();
            DrawText();
            DrawCursor();
            base.Draw(gameTime);
        }

        private void DrawScript()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(nullTex, new Rectangle(400, 20, 400, 565),
            new Color(new Vector4(0f, 0f, 0f, .62f)));
            spriteBatch.End();

            for (int i = scriptScroll; i < scriptScroll + 28; i++)
            {
                if (selScript == i)
                {
                    text.Color = Color.White;
                    text.DrawText(405, 25 + (i - scriptScroll) * 20, i.ToString() + " " + map.Scripts[i] + "*");
                }
                else
                {
                    if (text.DrawClickText(405, 25 + (i - scriptScroll) * 20, i.ToString() + ": " + map.Scripts[i],
                            mouseX, mouseY, mouseClick))
                    {
                        selScript = i;
                        editMode = EditingMode.Scripts;
                    }
                }

                if (map.Scripts[i].Length > 0)
                {
                    String[] split = map.Scripts[i].Split(' ');

                    int c = GetCommandColor(split[0]);
                    if (c > COLOR_NONE)
                    {
                        switch (c)
                        {
                            case COLOR_GREEN:
                                text.Color = Color.Lime;
                                break;
                            case COLOR_YELLOW:
                                text.Color = Color.Yellow;
                                break;
                        }
                        text.DrawText(405, 25 + (i - scriptScroll) * 20, i.ToString() + ": " + split[0]);
                    }
                }
                text.Color = Color.White;
                text.DrawText(405, 25 + (i - scriptScroll) * 20, i.ToString() + ": ");
            }
            bool mouseDown = (Mouse.GetState().LeftButton == ButtonState.Pressed);
            if (DrawButton(770, 20, 1, mouseX, mouseY, mouseDown) && scriptScroll > 0)
                scriptScroll--;
            if (DrawButton(770, 550, 2, mouseX, mouseY, mouseDown) && scriptScroll < map.Scripts.Length - 28)
                scriptScroll++;
        }

        private void DrawMapSegments()
        {
            Rectangle sRect = new Rectangle();
            Rectangle dRect = new Rectangle();
            text.Size = 0.8f;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(nullTex, new Rectangle(500, 20, 280, 550), new Color(0, 0, 0, 100));
            spriteBatch.End();
            for (int i = segScroll; i < segScroll + 9; i++)
            {
                SegmentDefinition segDef = map.SegmentDefinitions[i];
                if (segDef == null)
                    continue;
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                dRect.X = 500;
                dRect.Y = 50 + (i - segScroll) * 60;
                sRect = segDef.SourceRectangle;
                if (sRect.Width > sRect.Height)
                {
                    dRect.Width = 45;
                    dRect.Height = (int)(((float)sRect.Height / (float)sRect.Width) * 45.0f);
                }
                else
                {
                    dRect.Height = 45;
                    dRect.Width = (int)(((float)sRect.Width / (float)sRect.Height) * 45.0f);
                }
                spriteBatch.Draw(mapsTex[segDef.SourceIndex], dRect, sRect, Color.White);
                spriteBatch.End();
                text.Color = Color.White;
                text.DrawText(dRect.X + 50, dRect.Y, segDef.Name);
                if (rightMouseDown)
                {
                    if (mouseX > dRect.X && mouseX < 600 && mouseY > dRect.Y && mouseY < dRect.Y + 45)
                    {
                        if (mouseDragSeg == -1)
                        {
                            int f = map.AddSegment(curLayer, i);
                            if (f <= -1)
                                continue;
                            float layerScalar = 0.5f;
                            if (curLayer == 0)
                                layerScalar = 0.375f;
                            else if (curLayer == 2)
                                layerScalar = 0.625f;
                            map.Segments[curLayer, f].Location.X = (mouseX - sRect.Width / 4 + scroll.X * layerScalar);
                            map.Segments[curLayer, f].Location.Y = (mouseY - sRect.Height / 4 + scroll.Y * layerScalar);
                            mouseDragSeg = f;
                        }
                    }
                }
            }
            if (DrawButton(650, 20, 1, mouseState.X, mouseState.Y, (mouseState.LeftButton == ButtonState.Pressed && segScroll > 0)))
            { segScroll--; }
            if (DrawButton(650, 540, 2, mouseState.X, mouseState.Y, (mouseState.LeftButton == ButtonState.Pressed && segScroll < map.SegmentDefinitions.Length - 9)))
            { segScroll++; }

        }

        private void DrawCursor()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(iconsTex, new Vector2(mouseX, mouseY),
            new Rectangle(0, 0, 32, 32),
            Color.White, 0.0f,
            new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }

        private void DrawText()
        {
            string layerName = "map";
            switch (curLayer)
            {
                case 0:
                    layerName = "fondo";
                    break;
                case 1:
                    layerName = "media";
                    break;
                case 2:
                    layerName = "frente";
                    break;
            }
            if (text.DrawClickText(5, 5, "capa: " + layerName, mouseX, mouseY, mouseClick))
                curLayer = (curLayer + 1) % 3;
            switch (drawType)
            {
                case DrawingMode.SegmentSelection:
                    layerName = "dibujos";
                    break;
                case DrawingMode.CollitionMap:
                    layerName = "colisiones";
                    break;
                case DrawingMode.Ledge:
                    layerName = "bordes";
                    break;
                case DrawingMode.Script:
                    layerName = "Script";
                    break;
            }
            if (text.DrawClickText(5, 25, "dibujar: " + layerName, mouseX, mouseY, mouseClick))
                drawType = (DrawingMode)((int)(drawType + 1) % 4);


            text.Color = Color.White;
            if (editMode == EditingMode.Path)
                text.DrawText(5, 45, map.Path + "*");
            else
            {
                if (text.DrawClickText(5, 45, map.Path, mouseX, mouseY, mouseClick))
                    editMode = EditingMode.Path;
            }

            mouseClick = false;
        }

        private void DrawGrid()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    Rectangle dRect = new Rectangle(
                        x * 32 - (int)(scroll.X / 2),
                        y * 32 - (int)(scroll.Y / 2),
                        32,
                        32
                        );

                    if (x < 19)
                        spriteBatch.Draw(nullTex,
                            new Rectangle(dRect.X, dRect.Y, 32, 1),
                            new Color(255, 0, 0, 100));

                    if (y < 19)
                        spriteBatch.Draw(nullTex,
                            new Rectangle(dRect.X, dRect.Y, 1, 32),
                            new Color(255, 0, 0, 100));

                    if (x < 19 && y < 19)
                    {
                        if (map.Grid[x, y] == 1)
                            spriteBatch.Draw(nullTex, dRect,
                                new Color(255, 0, 0, 100));
                    }
                }
            }

            Color oColor = new Color(255, 255, 255, 100);
            spriteBatch.Draw(nullTex, new Rectangle(100, 50, 400, 1), oColor);
            spriteBatch.Draw(nullTex, new Rectangle(100, 50, 1, 500), oColor);
            spriteBatch.Draw(nullTex, new Rectangle(500, 50, 1, 500), oColor);
            spriteBatch.Draw(nullTex, new Rectangle(100, 550, 400, 1), oColor);

            spriteBatch.End();
        }

        private bool GetCanEdit()
        {
            if (mouseX > 100 && mouseX < 500 && mouseY > 100 && mouseY < 550)
                return true;
            return false;
        }

        private void DrawLedges()
        {
            Rectangle rect = new Rectangle();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            Color tColor = new Color();

            rect.X = 32;
            rect.Y = 0;
            rect.Width = 32;
            rect.Height = 32;

            for (int i = 0; i < 16; i++) //recorre todos los bordes (ledges)
            {
                if (map.Ledges[i] != null && map.Ledges[i].TotalNodes > 0)
                {
                    for (int n = 0; n < map.Ledges[i].TotalNodes; n++) //dibuja todos los nodos de cada borde(ledge)
                    {
                        Vector2 tVec;

                        tVec = map.Ledges[i].Nodes[n];
                        tVec -= scroll / 2.0f;
                        tVec.X -= 5.0f;

                        if (curLedge == i)
                            tColor = Color.Yellow;
                        else
                            tColor = Color.White;

                        spriteBatch.Draw(iconsTex, tVec, rect, tColor,
                            0.0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0.0f);

                        if (n < map.Ledges[i].TotalNodes - 1) //dibuja la pseudo-linea que une cada nodo
                        {
                            Vector2 nVec;

                            nVec = map.Ledges[i].Nodes[n + 1];
                            nVec -= scroll / 2.0f;
                            nVec.X -= 4.0f;

                            for (int x = 1; x < 20; x++)
                            {
                                Vector2 iVec = (nVec - tVec) * ((float)x / 20f) + tVec;

                                Color nColor = new Color(255, 255, 255, 75);

                                if (map.Ledges[i].Flags == 1)
                                    nColor = new Color(255, 0, 0, 75);

                                spriteBatch.Draw(iconsTex, iVec, rect, nColor, 0.0f, Vector2.Zero, 0.25f, SpriteEffects.None, 0.0f);
                            }
                        }
                    }
                }
            }

            spriteBatch.End();
        }

        private void DrawLedgePalette()
        {
            for (int i = 0; i < 16; i++)
            {
                if (map.Ledges[i] == null)
                    continue;

                int y = 50 + i * 20;
                if (curLedge == i)
                {
                    text.Color = Color.Lime;
                    text.DrawText(520, y, "borde " + i.ToString());
                }
                else
                {
                    if (text.DrawClickText(520, y, "borde " + i.ToString(),
                        mouseX, mouseY, mouseClick))
                        curLedge = i;
                }

                text.Color = Color.White;
                text.DrawText(620, y, "n" + map.Ledges[i].TotalNodes.ToString());

                if (text.DrawClickText(680, y, "f" + map.Ledges[i].Flags.ToString(), mouseX, mouseY, mouseClick))
                    map.Ledges[i].Flags = (map.Ledges[i].Flags + 1) % 2;
            }
        }

        private void UpdateKeys()
        {
            KeyboardState keyState = Keyboard.GetState();

            Keys[] currentKeys = keyState.GetPressedKeys();
            Keys[] lastKeys = oldKeyState.GetPressedKeys();

            bool found = false;

            for (int i = 0; i < currentKeys.Length; i++)
            {
                found = false;

                for (int y = 0; y < lastKeys.Length; y++)
                {
                    if (currentKeys[i] == lastKeys[y])
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    if (!EditingModeHelper.isInvalidKey(currentKeys[i]))
                    {
                        PressKey(currentKeys[i]);
                    }
                    else
                    {
                        currentKeys[i] = Keys.X;
                        PressKey(currentKeys[i]);
                    }
            }

            oldKeyState = keyState;
        }

        private void PressKey(Keys key)
        {
            string t = String.Empty;

            switch (editMode)
            {
                case EditingMode.Path:
                    t = map.Path;
                    break;
                case EditingMode.Scripts:
                    if (selScript < 0)
                        return;
                    t = map.Scripts[selScript];
                    break;
            }

            bool delLine = false;

            if (key == Keys.Back)
            {
                if (t.Length > 0) t = t.Substring(0, t.Length - 1);
                else if (editMode == EditingMode.Scripts)
                {
                    delLine = ScriptDelLine();
                }
            }
            else if (key == Keys.Enter)
            {
                if (editMode == EditingMode.Scripts)
                {
                    if (ScriptEnter())
                    {
                        t = "";
                    }
                }
                else
                    editMode = EditingMode.None;
            }
            else
            {
                t = (t + (char)key).ToLower();
            }

            switch (editMode)
            {
                case EditingMode.Path:
                    map.Path = t;
                    break;
            }


            if (!delLine)
            {
                switch (editMode)
                {
                    case EditingMode.Path:
                        map.Path = t;
                        break;
                    case EditingMode.Scripts:
                        map.Scripts[selScript] = t;
                        break;
                }
            }
            else
                selScript--;
        }

        private bool ScriptDelLine()
        {
            if (selScript <= 0)
                return false;
            for (int i = selScript; i < map.Scripts.Length - 1; i++)
                map.Scripts[i] = map.Scripts[i + 1];
            return true;
        }

        private bool ScriptEnter()
        {
            if (selScript >= map.Scripts.Length - 1)
                return false;
            for (int i = map.Scripts.Length - 1; i > selScript; i--)
                map.Scripts[i] = map.Scripts[i - 1];
            selScript++;
            return true;
        }

        private bool DrawButton(int x, int y, int index, int mosX, int mosY, bool mouseClick)
        {
            bool r = false;

            Rectangle sRect = new Rectangle(32 * (index % 8), 32 * (index / 8), 32, 32);
            Rectangle dRect = new Rectangle(x, y, 32, 32);

            if (dRect.Contains(mosX, mosY))
            {
                dRect.X -= 1;
                dRect.Y -= 1;
                dRect.Width += 2;
                dRect.Height += 2;

                if (mouseClick)
                    r = true;
            }

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(iconsTex, dRect, sRect, Color.White);
            spriteBatch.End();

            return r;
        }


        private int GetCommandColor(String s)
        {
            switch (s)
            {
                case "fog":
                case "monster":
                case "makebucket":
                case "addbucket":
                case "ifnotbucketgoto":
                case "wait":
                case "setflag":
                case "iftruegoto":
                case "iffalsegoto":
                case "setglobalflag":
                case "ifglobaltruegoto":
                case "ifglobalfalsegoto":
                case "stop":
                    return COLOR_GREEN;
                case "tag":
                    return COLOR_YELLOW;
            }
            return COLOR_NONE;
        } //para colorear los comandos del script




    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FuncWorks.XNA.XTiled;
using System.Diagnostics;
using System.Media;
using System.Text;

namespace ShiftWorld
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum State
	    {
            Menu = 0,
            LevelSelect = 1,
            Play = 2,
            Credits = 3,
            Instructions = 4
	    }
        State Game = State.Menu;
        int width = 1280, height = 768;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState keyboardState;
        MouseState mouseState;

        Rectangle StartGame = new Rectangle(950, 560, 320, 35);
        Rectangle Instructions = new Rectangle(950, 600, 250, 30);
        Rectangle Credits = new Rectangle(960, 635, 265, 35);
        Rectangle ExitGame = new Rectangle(970, 678, 105, 40);

        ObjectController objectController;
        ParticleController particleController;
        Rectangle mapView;
        List<Map> map;
        int mapIndex = 0;
        Camera2d camera = new Camera2d();
        Vector2 cameraPos;
        Vector2 eyesPos = new Vector2(170, 80);

        int dontUseThisTileIndexX;
        int dontUseThisTileIndexY;

        float beamCooldown;
        float dyingCountDown = 0;
        bool killed = false;


        Player player;
        Butterfly butterfly;
        Mist mist;
        Texture2D titlescreen;
        Texture2D instructions;
        Texture2D credits;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "ShiftWorld";
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
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

            // TODO: use this.Content to load your game content here

            
            camera.Zoom = 0.6f;



            player = new Player(Content.Load<Texture2D>("Textures/character animations"));
            butterfly = new Butterfly(Content.Load<Texture2D>("Textures/butterfly"));
            mist = new Mist(Content.Load<Texture2D>("Textures/sumu"), camera.Zoom);

            titlescreen = Content.Load<Texture2D>("Textures/title");
            credits = Content.Load<Texture2D>("Textures/credits screen");
            instructions = Content.Load<Texture2D>("Textures/instructions");



            objectController = new ObjectController(Content.Load<Texture2D>("Textures/BARREL"));
            particleController = new ParticleController(Content.Load<Texture2D>("Textures/Particle"), Content.Load<Texture2D>("Textures/sumu"));
            map = new List<Map>();
            map.Add(Content.Load<Map>("Maps/testing_map"));

            Reset();
            SwitchGameState(State.Menu);
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
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            Vector2 cameraDelta = new Vector2((float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f) * 200, 0);
            cameraPos = new Vector2(player.Position.X + 480/camera.Zoom, map[mapIndex].Height * map[mapIndex].TileHeight / 2.0f);
            camera.Pos = new Vector2((1 / camera.Zoom) * ((int)(cameraPos.X * camera.Zoom)), cameraPos.Y);
            //camera.Pos = new Vector2(cameraPos.X, cameraPos.Y);

            Vector2 RmousePosition = new Vector2(cameraPos.X + (mouseState.X - width / 2) / camera.Zoom, cameraPos.Y + (mouseState.Y - height / 2) / camera.Zoom);

            particleController.UpdateMouse(gameTime, cameraDelta, RmousePosition);



            switch (Game)
            {
                case State.Menu:
                    
                    if (keyboardState.IsKeyDown(Keys.Space))
                        SwitchGameState(State.Play);
                    if (StartGame.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed)
                        SwitchGameState(State.Play);
                    if (Instructions.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed)
                        SwitchGameState(State.Instructions);
                    if (Credits.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed)
                        SwitchGameState(State.Credits);
                    if (ExitGame.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed)
                        Exit();

                    break;

                case State.LevelSelect:
                    break;

                case State.Play:

                    player.Update(keyboardState, gameTime, cameraDelta);
                    butterfly.Update(camera.Pos, gameTime);
                    mist.Update(gameTime,camera.Pos);

                    particleController.Update(gameTime, cameraPos, RmousePosition);

                    HitBoxes(gameTime);

                    Beam(gameTime, RmousePosition);

                    dyingCountDown -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (player.HP() < 0 || player.Position.Y > (map[mapIndex].Height * map[mapIndex].TileHeight - player.Height))
                    {
                        player.Die();
                        if (!killed && dyingCountDown < -1000)
                        {
                            killed = true;
                            dyingCountDown = 2000;
                        }
                    }
                    if (dyingCountDown < 0 && killed)
                    {
                        Reset();
                    }

                    break;

                case State.Credits:
                    if (keyboardState.IsKeyDown(Keys.Space))
                        SwitchGameState(State.Menu);
                    break;



                default:
                    SwitchGameState(State.Menu);
                    break;
            }

           

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.NonPremultiplied,
                        null,
                        null,
                        null,
                        null,
                        camera.get_transformation(GraphicsDevice /*Send the variable that has your graphic device here*/));

            particleController.DrawMouse(spriteBatch);



            switch (Game)
            {
                case State.Menu:

                    spriteBatch.Draw(titlescreen, Vector2.Zero,null, Color.White,0.0f, Vector2.Zero,1/0.6f,SpriteEffects.None,0.1f);
                    break;

                case State.LevelSelect:
                    break;

                case State.Play:

                    DrawMapLayers(spriteBatch, mapIndex);
                    player.Draw(spriteBatch);
                    butterfly.Draw(spriteBatch);
                    mist.Draw(spriteBatch);
                    objectController.Draw(spriteBatch);
                    particleController.Draw(spriteBatch);

                    break;

                case State.Credits:

                    spriteBatch.Draw(credits, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 1/0.6f, SpriteEffects.None, 0.1f);

                    break;

                case State.Instructions:

                    spriteBatch.Draw(instructions, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 1 / 0.6f, SpriteEffects.None, 0.1f);

                    break;
            }



            particleController.DrawMouse(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawMapLayers(SpriteBatch spriteBatch, int mapIndex)
        {
            for (int i = 0; i < map[mapIndex].TileLayers.Count; ++i)
            {
                if (true)
                {
                    DrawLayer(spriteBatch, map[mapIndex], i, ref mapView, 0.9f - 0.001f * i);
                }
            }
        }

        public void DrawLayer(SpriteBatch spriteBatch, Map map, Int32 layerID, ref Rectangle region, Single layerDepth)
        {

            // Tiles are stored in a multidimensional array.
            // By converting the map coordinates to tile coordinates 
            // we can eliminate the need for bound checking
            Int32 txMin = region.X / map.TileWidth;
            Int32 txMax = (region.X + region.Width) / map.TileWidth;
            Int32 tyMin = region.Y / map.TileHeight;
            Int32 tyMax = (region.Y + region.Height) / map.TileHeight;

            for (int y = tyMin; y <= tyMax; y++)
            {
                for (int x = txMin; x <= txMax; x++)
                {

                    // check that we aren't going outside the map, and that there is a tile at this location
                    if (x < map.TileLayers[layerID].Tiles.Length && y < map.TileLayers[layerID].Tiles[x].Length
                        && map.TileLayers[layerID].Tiles[x][y] != null)
                    {
                        map.TileLayers[layerID].OpacityColor = Color.White;

                        // adjust the tiles map coordinates to screen space
                        Rectangle tileTarget = map.TileLayers[layerID].Tiles[x][y].Target;
                        tileTarget.X = tileTarget.X - region.X;
                        tileTarget.Y = tileTarget.Y - region.Y;

                        spriteBatch.Draw(
                            // the texture (image) of the tile sheet is mapped by
                            // Tile.SourceID -> TileLayers.TilesetID -> Map.Tileset.Texture
                            map.Tilesets[map.SourceTiles[map.TileLayers[layerID].Tiles[x][y].SourceID].TilesetID].Texture,

                            // screen space of the tile
                            tileTarget,

                            // source of the tile in the tilesheet
                            map.SourceTiles[map.TileLayers[layerID].Tiles[x][y].SourceID].Source,

                            // layers can have an opacity value, this property is Color.White at the opacity of the layer
                            map.TileLayers[layerID].OpacityColor,

                            // tile rotation value
                            map.TileLayers[layerID].Tiles[x][y].Rotation,

                            // origin of the tile, this is always the center of the tile
                            map.SourceTiles[map.TileLayers[layerID].Tiles[x][y].SourceID].Origin,

                            // tile horizontal or vertical flipping value
                            map.TileLayers[layerID].Tiles[x][y].Effects,

                            // depth for SpriteSortMode
                            layerDepth);
                    }
                }
            }
        }

        private void Beam(GameTime gameTime, Vector2 RMousePosition)
        {
            beamCooldown += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (beamCooldown > 500)
            {
                Vector2 direction = RMousePosition - player.Position;
                direction.Normalize();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    beamCooldown = 0;
                    particleController.AddBeam(player.Position + eyesPos, direction, true);
                }
                else if (mouseState.RightButton == ButtonState.Pressed)
                {
                    beamCooldown = 0;
                    particleController.AddBeam(player.Position + eyesPos, direction, false);
                }
            }
        }

        private void HitBoxes(GameTime gameTime)
        {

            for (int i = 0; i < 3; i++)
            {
                float leftBox = 96, rightBox = 192, topBox = 0, botBox = 254;
                float widthDif = (rightBox - leftBox) / 2.2f;
                float heightDif = (botBox - topBox) / 4f;
                float midWidht = (rightBox + leftBox) / 2 - widthDif;
                float midHeight = (botBox + topBox) / 2 - heightDif;



                TileIndexX = (int)(player.Position.X + midWidht + widthDif * i) / map[mapIndex].TileWidth;
                TileIndexY = (int)(player.Position.Y + botBox) / map[mapIndex].TileHeight;

                if (map[mapIndex].TileLayers[0].Tiles[TileIndexX][TileIndexY] != null) // going down
                {
                    if (player.Velocity.Y > 0)
                    {
                        player.Position = new Vector2(
                            player.Position.X,
                            map[mapIndex].TileLayers[0].Tiles[TileIndexX][TileIndexY].Target.Y -
                            map[mapIndex].TileHeight / 2 - botBox);

                        player.Land();
                    }
                }

                TileIndexX = (int)(player.Position.X + midWidht + widthDif * i) / map[mapIndex].TileWidth;
                TileIndexY = (int)(player.Position.Y + topBox) / map[mapIndex].TileHeight;

                if (map[mapIndex].TileLayers[0].Tiles[TileIndexX][TileIndexY] != null) // going up
                {
                    player.Position = new Vector2(
                        player.Position.X,
                        map[mapIndex].TileLayers[0].Tiles[TileIndexX][TileIndexY].Target.Y +
                        map[mapIndex].TileHeight / 2 + topBox);
                }

                TileIndexX = (int)(player.Position.X + rightBox) / map[mapIndex].TileWidth;
                TileIndexY = (int)(player.Position.Y + midHeight + heightDif * i) / map[mapIndex].TileHeight;

                if (map[mapIndex].TileLayers[0].Tiles[TileIndexX][TileIndexY] != null) // going right
                {
                    player.Position = new Vector2(
                        map[mapIndex].TileLayers[0].Tiles[TileIndexX][TileIndexY].Target.X -
                        map[mapIndex].TileWidth / 2 - rightBox
                        , player.Position.Y);

                    player.HP((float)-gameTime.ElapsedGameTime.TotalMilliseconds);
                }
                else
                {
                    player.HP(5);
                }

                TileIndexX = (int)(player.Position.X + leftBox) / map[mapIndex].TileWidth;
                TileIndexY = (int)(player.Position.Y + midHeight + heightDif * i) / map[mapIndex].TileHeight;

                if (map[mapIndex].TileLayers[0].Tiles[TileIndexX][TileIndexY] != null) // going left
                {
                    player.Position = new Vector2(
                        map[mapIndex].TileLayers[0].Tiles[TileIndexX][TileIndexY].Target.X +
                        map[mapIndex].TileWidth/2 - leftBox
                        , player.Position.Y);
                }
            }
        }

        private int TileIndexX
        {
            get { return dontUseThisTileIndexX; }
            set { dontUseThisTileIndexX = (value >= 0 && value < map[mapIndex].Width) ? value : 0; }
        }

        private int TileIndexY
        {
            get { return dontUseThisTileIndexY; }
            set { dontUseThisTileIndexY = (value >= 0 && value < map[mapIndex].Height) ? value : 0; }
        }

        private void SwitchGameState(State GameState)
        {
            switch (GameState)
            {
                case State.Menu:
                    player.Position = new Vector2(width * camera.Zoom / 2 - 116, height / 2);
                    Game = State.Menu;
                    break;

                case State.Play:
                    Reset();
                    Game = State.Play;
                    break;

                case State.Credits:
                    Game = State.Credits;
                    break;
            }
        }

        private void Reset()
        {
            mapView = map[mapIndex].Bounds;

            killed = false;
            dyingCountDown = 0;

            player.Reset();
            particleController.Reset();

            objectController.Reset();
            foreach (var l in map[mapIndex].ObjectLayers)
            {
                foreach (var o in l.MapObjects)
                {
                    if (o.Name == "Barrel")
                    {
                        objectController.AddObject(1, new Vector2(o.Bounds.X, o.Bounds.Y));
                    }
                }
            }
        }
    }
}

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
        int width = 1280, height = 768;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState keyboardState;
        MouseState mouseState;

        Rectangle mapView;
        List<Map> map;
        int mapIndex = 0;
        Camera2d camera = new Camera2d();
        Vector2 cameraPos;
        int dontUseThisTileIndexX;
        int dontUseThisTileIndexY;

        Player player;

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
            mapView = graphics.GraphicsDevice.Viewport.Bounds;

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





            player = new Player(Content.Load<Texture2D>("Textures/character animations"));




            map = new List<Map>();
            map.Add(Content.Load<Map>("Maps/map_testing"));
            mapView = map[0].Bounds;

            cameraPos = new Vector2(width, map[mapIndex].Height * map[mapIndex].TileHeight / 2);
            camera.Pos = new Vector2((int)cameraPos.X, (int)cameraPos.Y);
            camera.Zoom = 1f;
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

            Vector2 cameraDelta = new Vector2((float)(gameTime.ElapsedGameTime.TotalMilliseconds/1000.0f) * 200,0);
            //cameraPos += cameraDelta;
            cameraPos = player.Position;

            //camera.Pos = new Vector2(cameraPos.X, cameraPos.Y);
            camera.Pos = new Vector2((int)cameraPos.X, cameraPos.Y);




            
            player.Update(keyboardState, gameTime, cameraDelta, cameraPos);

            if (player.HP() < 0 || player.Position.Y > (map[mapIndex].Height * map[mapIndex].TileHeight - player.Height))
            {
                //this.Exit();
            }




            HitBoxes(gameTime);

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
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        camera.get_transformation(GraphicsDevice /*Send the variable that has your graphic device here*/));

            DrawMapLayers(spriteBatch, mapIndex);

            player.Draw(spriteBatch);





            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawMapLayers(SpriteBatch spriteBatch, int mapIndex)
        {
            for (int i = 0; i < map[mapIndex].TileLayers.Count; ++i)
            {
                if (true)
                {
                    DrawLayer(spriteBatch, map[mapIndex], i, ref mapView, 0.1f - 0.001f * i);
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

        private void HitBoxes(GameTime gameTime)
        {

            for (int i = 0; i < 3; i++)
            {
                float leftBox = 96, rightBox = 192, topBox = 0, botBox = 255;
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
    }
}

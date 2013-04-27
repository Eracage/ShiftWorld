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
        int width = 1280, height = 720;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState keyboardState;

        Rectangle mapView;
        List<Map> map;
        int mapIndex = 0;
        Camera2d camera = new Camera2d();
        Vector2 cameraPos;

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
            map = new List<Map>();

            cameraPos = new Vector2(width / 2, height / 2);
            camera.Pos = new Vector2((int)cameraPos.X, (int)cameraPos.Y);





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





            player = new Player(Content.Load<Texture2D>("Textures/character size"));




            map.Add(Content.Load<Map>("Maps/stage"));
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

            Vector2 cameraDelta = new Vector2((float)(gameTime.ElapsedGameTime.TotalMilliseconds/1000.0f)*50,0);
            cameraPos += cameraDelta;
            camera.Pos = new Vector2(cameraPos.X, cameraPos.Y);




            
            player.Update(keyboardState, gameTime, cameraDelta);





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
            for (int i = 0; i < map[0].TileLayers.Count; ++i)
            {
                DrawLayer(spriteBatch, map[mapIndex], i, ref mapView, 0.1f - 0.001f * i);
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
    }
}

﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;

namespace pacman {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        SpriteBatch sb;
        gameMap map;
        public static double scaling = 1;
        static double originalWidth = 784;
        static double originalHeight = 1008;
        static double width = originalWidth;
        static double height = originalHeight;
        static double proportions = height / width;
        pacman torsten;
        ghost spöke;
        Song pacsong;
        SpriteFont scoreFont;
        Texture2D spriteMap;

        public static int score = 0;
        public static int scoreCount = 0;
        public static int maxScoreCount = 0;
        public static bool gameFinished;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
        }

        public void OnResize(Object sender, EventArgs e) {
            if(width != GraphicsDevice.PresentationParameters.BackBufferWidth) {
                width = GraphicsDevice.PresentationParameters.BackBufferWidth;
                height = Math.Round(width * proportions);
            } else {
                height = GraphicsDevice.PresentationParameters.BackBufferHeight;
                width = Math.Round(height / proportions);
            }
            scaling = width / originalWidth;
            _graphics.PreferredBackBufferWidth = (int)width;
            _graphics.PreferredBackBufferHeight = (int)height;
            _graphics.ApplyChanges();
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            
            _graphics.PreferredBackBufferWidth = (int)width;
            _graphics.PreferredBackBufferHeight = (int)height;
            _graphics.ApplyChanges();

            sb = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent() {

            _graphics.PreferredBackBufferWidth = (int)width;
            _graphics.PreferredBackBufferHeight = (int)height;
            _graphics.ApplyChanges();

            spriteMap = Content.Load<Texture2D>("spriteMap_pacman");

            torsten = new pacman(this, 13, 26, 2, spriteMap);
            spöke = new ghost(this, 13, 14, 2);

            map = new gameMap(this, 28, 36, spriteMap);
            map.InitializeWalls();
            torsten.init();
            this.pacsong = Content.Load<Song>("PACMAN_intro");
            MediaPlayer.Play(this.pacsong);

            scoreFont = Content.Load<SpriteFont>("scoreFont");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            

            // TODO: Add your update logic here
            if(!gameFinished) {
                spöke.Update(map, torsten);
                torsten.Update(map);
            }


            if(scoreCount == maxScoreCount) {
                gameFinished = true;
            }     

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            map.Draw(gameTime);
            spöke.Draw(gameTime);
            torsten.Draw(gameTime);

            sb.Begin();
            sb.DrawString(scoreFont, "Score: " + score.ToString(), new Vector2((int)Math.Round(620 * scaling), (int)Math.Round(960 * scaling)), Color.White);
            if(gameFinished) {
                sb.DrawString(scoreFont, "You won!!!", new Vector2((int)Math.Round(350 * scaling), (int)Math.Round(50 * scaling)), Color.Yellow);
            }
            sb.End();
            base.Draw(gameTime);
        }
    }
}

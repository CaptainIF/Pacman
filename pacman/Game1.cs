using Microsoft.Xna.Framework;
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
        ghost[] spöken = new ghost[2];
        Song pacsong;
        SpriteFont scoreFont;
        SpriteFont menuFont;
        Texture2D spriteMap;
        public string gameState = "playing";

        public static int score = 0;
        public static int scoreCount = 0;
        public static int maxScoreCount = 0;
        public static bool gameFinished;
        public static bool gameOver;
        public static bool rageMode = false;
        public static bool ghostAlive = true;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
        }

        public void startRage() {
            rageMode = true;
            for(int i = 0;i < spöken.Length;i++) {
                spöken[i].mode = "frightened";
            }
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

            spöken[0] = new ghost(this, 13, 14, 2, "blinky", map);
            spöken[1] = new ghost(this, 16, 14, 2, "pinky", map);
            //spöke = new ghost(this, 13, 14, 2, map);
            torsten = new pacman(this, 13, 26, 2, spöken, spriteMap);

            map = new gameMap(this, 28, 36, spriteMap);
            map.InitializeWalls();
            torsten.init();
            this.pacsong = Content.Load<Song>("PACMAN_intro");
            
            MediaPlayer.Play(this.pacsong);

            scoreFont = Content.Load<SpriteFont>("scoreFont");
            menuFont = Content.Load<SpriteFont>("menuFont");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            

            // TODO: Add your update logic here
            if(gameState == "playing") {

            if(!gameFinished && !gameOver) {
                for(int i = 0;i < spöken.Length;i++) {
                    spöken[i].Update(map, torsten);
                }
                torsten.Update(map);
            }
            

            for(int i = 0; i < spöken.Length;i++) {
                if ((torsten.currentI == spöken[i].currentI) && (torsten.currentJ == spöken[i].currentJ)) {
                    if (spöken[i].mode == "frightened") {
                        spöken[i].ghostDied();
                    } else if(spöken[i].mode == "chase" || spöken[i].mode == "scatter") {
                        gameOver = true;
                    }
                }
            }



                if (scoreCount == maxScoreCount) {
                gameFinished = true;
            }     

            base.Update(gameTime);
            } else if(gameState == "menu") {
                
            }
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            if(gameState == "playing") {
            map.Draw(gameTime);
            for(int i = 0;i < spöken.Length;i++) {
                spöken[i].Draw(gameTime);
            }
            torsten.Draw(gameTime);
            
            sb.Begin();
            sb.DrawString(scoreFont, "Score: " + score.ToString(), new Vector2((int)Math.Round(620 * scaling), (int)Math.Round(960 * scaling)), Color.White);
            sb.DrawString(scoreFont, spöken[0].mode, new Vector2(100, 20), Color.White);
            sb.DrawString(scoreFont, spöken[0].stateTimer.Elapsed.ToString(), new Vector2(200, 20), Color.White);
            sb.DrawString(scoreFont, spöken[1].mode, new Vector2(100, 70), Color.White);
            sb.DrawString(scoreFont, spöken[1].stateTimer.Elapsed.ToString(), new Vector2(200, 70), Color.White);


            if (gameFinished) {
                sb.DrawString(scoreFont, "You won!!!", new Vector2((int)Math.Round(350 * scaling), (int)Math.Round(50 * scaling)), Color.Yellow);
            }

            if (gameOver) {
                sb.DrawString(scoreFont, "GAME OVER!!!", new Vector2((int)Math.Round(350 * scaling), (int)Math.Round(50 * scaling)), Color.Yellow);
            }

            if (rageMode) {
                sb.DrawString(scoreFont, "Rage!!!", new Vector2(350, 50), Color.Purple);
            }

            sb.End();
            base.Draw(gameTime);
            } else if(gameState == "menu") {
                sb.Begin();
                sb.DrawString(menuFont, "PACMAN", new Vector2(340, 50), Color.Yellow);
                sb.DrawString(menuFont, "START GAME", new Vector2(310, 150), Color.Red);
                sb.End();
                base.Draw(gameTime);
            }

        }
    }
}

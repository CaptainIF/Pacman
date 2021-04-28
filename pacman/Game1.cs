using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace pacman {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        SpriteBatch sb;
        gameMap map;
        const int width = 784;
        const int height = 1008;
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
            
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();

            sb = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent() {

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            map.Draw(gameTime);
            spöke.Draw(gameTime);
            torsten.Draw(gameTime);

            sb.Begin();
            sb.DrawString(scoreFont, "Score: " + score.ToString(), new Vector2(620, 960), Color.White);
            if(gameFinished) {
                sb.DrawString(scoreFont, "You won!!!", new Vector2(350, 50), Color.Yellow);
            }
            sb.End();
            base.Draw(gameTime);
        }
    }
}

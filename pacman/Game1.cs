using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pacman {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        gameMap map;
        const int width = 784;
        const int height = 1008;
        pacman torsten;

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

            torsten = new pacman(this, 13, 26, 2);

            base.Initialize();
            
        }

        protected override void LoadContent() {
            
            map = new gameMap(this, 28, 36);
            map.InitializeWalls();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            torsten.Update(map);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            map.Draw(gameTime);
            torsten.Draw(gameTime);
            

            base.Draw(gameTime);
        }
    }
}

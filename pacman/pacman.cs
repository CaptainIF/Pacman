using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace pacman {
    class pacman : DrawableGameComponent {

        public int x, y;
        public int speed;

        public pacman(Game game, int x, int y, int speed):base(game) {
            this.x = x;
            this.y = y;
            this.speed = speed;
        }

        public void Update(gameMap map) {
            var kstate = Keyboard.GetState();

            if(kstate.IsKeyDown(Keys.Up)) {
                this.y -= this.speed;
            }

            if (kstate.IsKeyDown(Keys.Down)) {
                this.y += this.speed;
            }

            if (kstate.IsKeyDown(Keys.Left)) {
                this.x -= this.speed;
            }

            if (kstate.IsKeyDown(Keys.Right)) {
                this.x += this.speed;
            }
        }

        public void Draw(GameTime gt) {
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            Texture2D texture;
            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.DarkSlateGray });

            
            sb.Begin();
            sb.Draw(texture, new Rectangle(this.x, this.y, 50, 50), Color.White);
            sb.End();
        }

    }
}

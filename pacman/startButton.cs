
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Timers;

namespace pacman {
    class startButton : DrawableGameComponent {
        public int width;
        public int height;
        public Vector2 pos;
        Texture2D texture;


        public startButton(Game game, int width, int height) : base(game) {

            this.width = width;
            this.height = height;
            this.pos.X = 350;
            this.pos.Y = 150;
            


        }

        public void Draw(GameTime gt) {
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            this.texture = Game.Content.Load<Texture2D>("spöke_1");

            sb.Begin();
            sb.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, width, height), Color.CornflowerBlue);
        }
    }
}
    


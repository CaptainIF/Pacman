
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Timers;

namespace pacman {
    class button : DrawableGameComponent {
        public int width;
        public int height;
        public Vector2 pos;


        public button(Game game) : base(game) {

            this.width = 300;
            this.height = 200;
            this.pos.X = 350;
            this.pos.Y = 150;


        }

        public void Draw(GameTime gt) {
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            //Texture2D texture = new Rectangle();

            //sb.Begin();
            //sb.Draw(Rectangle)
        }
    }
}
    


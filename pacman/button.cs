
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Timers;

namespace pacman {
    public class button : DrawableGameComponent {
        public int width;
        public int height;
        public Vector2 pos;
        Texture2D texture;
        Game game;
        string text;


        public button(Game game, string text, int x, int y, int width, int height) : base(game) {
            this.pos.X = x;
            this.pos.Y = y;
            this.width = width;
            this.height = height;
            this.text = text;
            this.game = game;
        }

        public void update() {
            if(Mouse.GetState().LeftButton == ButtonState.Pressed) {
                if(Mouse.GetState().X > pos.X && Mouse.GetState().X < pos.X + width && Mouse.GetState().Y > pos.Y && Mouse.GetState().Y < pos.Y + height) {
                    Game1.gameState = "playing";
                }
            }
        }

        public void Draw(GameTime gt) {
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);

            sb.Begin();
            sb.DrawString(Game1.menuFont, text, pos, Color.Red);
            sb.End();
        }
    }
}
    


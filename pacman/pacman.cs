using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace pacman {
    class pacman : DrawableGameComponent {

        public Vector2 pos;
        public int currentI, currentJ;
        public Vector2 dir;
        public int speed;
        public int width = 50;

        public pacman(Game game, int i, int j, int speed):base(game) {
            this.pos.X = i * 28 - 28 / 2;
            this.pos.Y = j * 28 - 28 / 2;
            this.dir.X = 0;
            this.dir.Y = 0;
            this.speed = speed;
        }

        public void Update(gameMap map) {
            this.currentI = (int)((this.pos.X) / map.tiles[0, 0].size);
            this.currentJ = (int)((this.pos.Y) / map.tiles[0, 0].size);

            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up)) {
                if (map.tiles[this.currentI, this.currentJ - 1].tileID == 1) {
                    this.dir.X = 0;
                    this.dir.Y = -1;
                }
            } else if (kstate.IsKeyDown(Keys.Down)) {
                if (map.tiles[this.currentI, this.currentJ + 1].tileID == 1) {
                    this.dir.X = 0;
                    this.dir.Y = 1;
                }
            } else if (kstate.IsKeyDown(Keys.Left)) {
                if (map.tiles[this.currentI - 1, this.currentJ].tileID == 1) {
                    this.dir.X = -1;
                    this.dir.Y = 0;
                }
            } else if (kstate.IsKeyDown(Keys.Right)) {
                if (map.tiles[this.currentI, this.currentJ + 1].tileID == 1) {
                    this.dir.X = 1;
                    this.dir.Y = 0;
                }
            }

            Debug.WriteLine(this.dir.X * 14);
            if (map.tiles[(int)((this.pos.X /*+ this.dir.X * 28*/ + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 1) {
                this.pos.X += this.dir.X;
                this.pos.Y += this.dir.Y;
            }
            
        }

        public void Draw(GameTime gt) {
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            Texture2D texture;
            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.DarkSlateGray });

            
            sb.Begin();
            sb.Draw(texture, new Rectangle((int)this.pos.X, (int)this.pos.Y, this.width, this.width), Color.White);
            sb.End();
        }

    }
}

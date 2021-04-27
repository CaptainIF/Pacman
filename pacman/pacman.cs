﻿using Microsoft.Xna.Framework;
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

        public pacman(Game game, int i, int j, int speed) : base(game) {
            this.pos.X = i * 28 + 28 / 2;
            this.pos.Y = j * 28 + 28 / 2;
            this.dir.X = 0;
            this.dir.Y = 0;
            this.speed = speed;
        }

        public void Update(gameMap map) {
            this.currentI = (int)((this.pos.X) / map.tiles[0, 0].size);
            this.currentJ = (int)((this.pos.Y) / map.tiles[0, 0].size);

            if(map.tiles[currentI, currentJ].tileID == 2) {
                map.tiles[currentI, currentJ].tileID = 1;
                map.tiles[currentI, currentJ].Init();
                Game1.score += 10;
                Game1.scoreCount++;
            }

            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.W)) {
                if ((map.tiles[this.currentI, this.currentJ - 1].tileID == 1 || map.tiles[this.currentI, this.currentJ - 1].tileID == 2) && this.pos.X % 28 < 14 + this.speed / 2 && this.pos.X % 28 > 14 - this.speed / 2) {
                    this.dir.X = 0;
                    this.dir.Y = -1;
                }
            }
            if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S)) {
                if ((map.tiles[this.currentI, this.currentJ + 1].tileID == 1 || map.tiles[this.currentI, this.currentJ + 1].tileID == 2) && this.pos.X % 28 < 14 + this.speed / 2 && this.pos.X % 28 > 14 - this.speed / 2) {
                    this.dir.X = 0;
                    this.dir.Y = 1;
                }
            }
            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A)) {
                if ((map.tiles[this.currentI - 1, this.currentJ].tileID == 1 || map.tiles[this.currentI - 1, this.currentJ].tileID == 2) && this.pos.Y % 28 < 14 + this.speed / 2 && this.pos.Y % 28 > 14 - this.speed / 2) {
                    this.dir.X = -1;
                    this.dir.Y = 0;
                }
            }
            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D)) {
                if ((map.tiles[this.currentI + 1, this.currentJ].tileID == 1 || map.tiles[this.currentI + 1, this.currentJ].tileID == 2) && this.pos.Y % 28 < 14 + this.speed / 2 && this.pos.Y % 28 > 14 - this.speed / 2) {
                    this.dir.X = 1;
                    this.dir.Y = 0;
                }
            }

            if (map.tiles[(int)((this.pos.X /*+ this.dir.X * 28*/ + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 1 || map.tiles[(int)((this.pos.X /*+ this.dir.X * 28*/ + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 2) {
                this.pos.X += this.dir.X * this.speed;
                this.pos.Y += this.dir.Y * this.speed;
            } else {
                this.dir.X = 0;
                this.dir.Y = 0;
                this.pos.X = this.currentI * 28 + 14;
                this.pos.Y = this.currentJ * 28 + 14;
            }

        }

        public void Draw(GameTime gt) {
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            Texture2D texture;
            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.DarkSlateGray });


            sb.Begin();
            sb.Draw(texture, new Rectangle((int)this.pos.X - this.width / 2, (int)this.pos.Y - this.width / 2, this.width, this.width), Color.White);
            sb.End();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace pacman {
    class ghost : DrawableGameComponent {
        public int speed;
        public Vector2 dir;
        public Vector2 pos;
        public int width = 50;
        public int currentI, currentJ;
        public Texture2D texture;

        public ghost(Game game, int i, int j,int speed):base(game) {

            this.speed = speed;
            this.pos.X = i * 28 + 14;
            this.pos.Y = j * 28 + 14;
            this.dir.X = -1;
            this.dir.Y = 0;
        }

        public void Update(gameMap map, pacman torsten) {
            this.currentI = (int)((this.pos.X) / map.tiles[0, 0].size);
            this.currentJ = (int)((this.pos.Y) / map.tiles[0, 0].size);
            var kstate = Keyboard.GetState();
            var neighbour = map.tiles[currentI, currentJ].CheckNeighbours(map);



            if (neighbour.Count == 2) {

                if (map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 1
                    || map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 2) {
                    this.pos.X += this.dir.X * this.speed;
                    this.pos.Y += this.dir.Y * this.speed;
                }

                else {
                    Debug.WriteLine("hello");
                    this.pos.X = this.currentI * 28 + 14;
                    this.pos.Y = this.currentJ * 28 + 14;

                    if (map.tiles[this.currentI, this.currentJ + 1].tileID == 0 && (map.tiles[this.currentI + 1, this.currentJ].tileID == 0 || map.tiles[this.currentI - 1, this.currentJ].tileID == 0) && this.dir.X != 0) {
                        this.dir.X = 0;
                        this.dir.Y = -1;

                        Debug.WriteLine("UP");
                    }
                    else if (map.tiles[this.currentI, this.currentJ - 1].tileID == 0 && (map.tiles[this.currentI + 1, this.currentJ].tileID == 0 || map.tiles[this.currentI - 1, this.currentJ].tileID == 0) && this.dir.X != 0) {
                        this.dir.X = 0;
                        this.dir.Y = 1;

                        Debug.WriteLine("DOWN");

                    }
                    else if (map.tiles[this.currentI + 1, this.currentJ].tileID == 0 && (map.tiles[this.currentI, this.currentJ - 1].tileID == 0 || map.tiles[this.currentI, this.currentJ + 1].tileID == 0) && this.dir.Y != 0) {
                        this.dir.X = -1;
                        this.dir.Y = 0;

                        Debug.WriteLine("LEFT");

                    }
                    else if (map.tiles[this.currentI - 1, this.currentJ].tileID == 0 && (map.tiles[this.currentI, this.currentJ - 1].tileID == 0 || map.tiles[this.currentI, this.currentJ + 1].tileID == 0) && this.dir.Y != 0) {
                        this.dir.X = 1;
                        this.dir.Y = 0;

                        Debug.WriteLine("RIGHT");

                    }

                }



            } else if(neighbour.Count == 1) {

            }

            
                   
        }

        public void Draw(GameTime gt) {
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            this.texture = Game.Content.Load<Texture2D>("spöke_1");

            sb.Begin();
            sb.Draw(this.texture, new Rectangle((int)this.pos.X - this.width / 2, (int)this.pos.Y - this.width / 2, this.width, this.width), Color.White);
            sb.End();
        }

    }
}

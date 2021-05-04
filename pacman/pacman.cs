using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public Texture2D spriteMap;
        public Texture2D[] pacmanTextures = new Texture2D[4];
        public int currentTexture = 3;
        public int textureSpeed = 7;
        public int textureCount = 0;
        public float rotation = 0;
        public SoundEffect chomp;
        public SoundEffectInstance chompInstance;

        public pacman(Game game, int i, int j, int speed, Texture2D spriteMap) : base(game) {
            this.pos.X = i * 28 + 28 / 2;
            this.pos.Y = j * 28 + 28 / 2;
            this.dir.X = 0;
            this.dir.Y = 0;
            this.speed = speed;
            this.spriteMap = spriteMap;
            chomp = Game.Content.Load<SoundEffect>("chomp");
            chompInstance = chomp.CreateInstance();
            chompInstance.Volume = 0.1f;
        }

        public void init() {
            Rectangle source = new Rectangle(0, 28, this.width, this.width);
            pacmanTextures[0] = new Texture2D(GraphicsDevice, source.Width, source.Height);
            Color[] data = new Color[source.Width * source.Height];
            spriteMap.GetData(0, source, data, 0, data.Length);
            pacmanTextures[0].SetData(data);

            source = new Rectangle(this.width, 28, this.width, this.width);
            pacmanTextures[1] = new Texture2D(GraphicsDevice, source.Width, source.Height);
            data = new Color[source.Width * source.Height];
            spriteMap.GetData(0, source, data, 0, data.Length);
            pacmanTextures[1].SetData(data);

            source = new Rectangle(this.width * 2, 28, this.width, this.width);
            pacmanTextures[2] = new Texture2D(GraphicsDevice, source.Width, source.Height);
            data = new Color[source.Width * source.Height];
            spriteMap.GetData(0, source, data, 0, data.Length);
            pacmanTextures[2].SetData(data);

            source = new Rectangle(this.width, 28, this.width, this.width);
            pacmanTextures[3] = new Texture2D(GraphicsDevice, source.Width, source.Height);
            data = new Color[source.Width * source.Height];
            spriteMap.GetData(0, source, data, 0, data.Length);
            pacmanTextures[3].SetData(data);
        }

        public void Update(gameMap map) {
            this.currentI = (int)((this.pos.X) / map.tiles[0, 0].size);
            this.currentJ = (int)((this.pos.Y) / map.tiles[0, 0].size);

            if((int)((this.pos.X + (this.dir.X * 14)) / 28) < 0) {
                this.currentI = 27;
                this.pos.X = currentI * 28;
            } else if((int)((this.pos.X + (this.dir.X * 14)) / 28) > 27) {
                this.currentI = 0;
                this.pos.X = currentI * 28;
            }

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
                    this.rotation = (float)Math.PI + (float)Math.PI / 2;
                }
            }
            if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S)) {
                if ((map.tiles[this.currentI, this.currentJ + 1].tileID == 1 || map.tiles[this.currentI, this.currentJ + 1].tileID == 2) && this.pos.X % 28 < 14 + this.speed / 2 && this.pos.X % 28 > 14 - this.speed / 2) {
                    this.dir.X = 0;
                    this.dir.Y = 1;
                    this.rotation = (float)Math.PI / 2;
                }
            }
            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A)) {
                if ((map.tiles[this.currentI - 1, this.currentJ].tileID == 1 || map.tiles[this.currentI - 1, this.currentJ].tileID == 2) && this.pos.Y % 28 < 14 + this.speed / 2 && this.pos.Y % 28 > 14 - this.speed / 2) {
                    this.dir.X = -1;
                    this.dir.Y = 0;
                    this.rotation = (float)Math.PI;
                }
            }
            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D)) {
                if ((map.tiles[this.currentI + 1, this.currentJ].tileID == 1 || map.tiles[this.currentI + 1, this.currentJ].tileID == 2) && this.pos.Y % 28 < 14 + this.speed / 2 && this.pos.Y % 28 > 14 - this.speed / 2) {
                    this.dir.X = 1;
                    this.dir.Y = 0;
                    this.rotation = 0;
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

            if(this.dir.X != 0 || this.dir.Y != 0) {
                textureCount++;
                if (textureCount == textureSpeed) {
                    currentTexture++;
                    textureCount = 0;
                }

                if (currentTexture == pacmanTextures.Length) {
                    currentTexture = 0;
                }

                if(chompInstance.State != SoundState.Playing) {
                    chompInstance.Play();
                }
            } else {
                currentTexture = 0;
                textureCount = 0;
            }

        }

        public void Draw(GameTime gt) {
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            Texture2D texture;
            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.DarkSlateGray });


            sb.Begin();
            sb.Draw(pacmanTextures[this.currentTexture], new Vector2((int)Math.Round(this.pos.X * Game1.scaling), (int)Math.Round(this.pos.Y * Game1.scaling)), null, Color.White, this.rotation, new Vector2((int)Math.Round((this.width * Game1.scaling) / 2), (int)Math.Round((this.width * Game1.scaling) / 2)), (float)Game1.scaling, SpriteEffects.None, 0);
            sb.End();
        }

    }
}

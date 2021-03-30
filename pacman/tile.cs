using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;

namespace pacman {
    class tile : DrawableGameComponent {
        int size = 28;
        int wallID = 0;
        public Vector2 position;
        public int tileID;
        Texture2D tileTexture;
        
        public tile(Game game): base(game) {}
        public void Initialize(Vector2 position, int tileID) {
            this.position = position;
            this.tileID = tileID;
            Texture2D allTiles = Game.Content.Load<Texture2D>("tilemap");
            Rectangle source = new Rectangle(size * tileID, size*1, size, size);
            tileTexture = new Texture2D(GraphicsDevice, source.Width, source.Height);
            Color[] data = new Color[source.Width * source.Height];
            allTiles.GetData(0, source, data, 0, data.Length);
            tileTexture.SetData(data);
        }

        public void CalculateWalls(gameMap map) {
            if(tileID == 1) {
                Debug.WriteLine(this.position.X.ToString() + ", " + this.position.Y.ToString());
            }
        }

        public tile[] CheckNeighbours(gameMap map) {
            tile[,] grid = map.tiles;
            int i = (int)this.position.X;
            int j = (int)this.position.Y;
            tile[] result;
            if(grid[i, j].tileID == 1) {
                result[result.Length] = grid[i, j];
            }


            return result;
        }

        public void LoadTexture() {

        }

        public void Draw(SpriteBatch sb) {
            //RenderTarget2D target = new RenderTarget2D(GraphicsDevice, size, size);
            Rectangle target = new Rectangle((int)position.X * size, (int)position.Y * size, size, size);
            sb.Draw(tileTexture, target, Color.White);
        }

    }
    class gameMap : DrawableGameComponent {
        public int width, height;
        public tile[,] tiles;
        public gameMap(Game game,int width, int height):base(game) {
            this.width = width;
            this.height = height;
            tiles = new tile[28, 36];

           
            using (StringReader testSR = new StringReader(Properties.Resources.pacmapText)) {
                for (int j = 0; j < tiles.GetLength(1); j++) {
                    for (int i = 0; i < tiles.GetLength(0); i++) {
                        int symbol = testSR.Read() - 0x30;
                        tile t = new tile(game);
                        t.Initialize(new Vector2(i, j), symbol);
                        tiles[i, j] = t;  
                    }
                    testSR.Read();
                    testSR.Read();//fixar radbrytningar
                }
            }
        }

        public void InitializeWalls() {
            for(int i = 0;i<tiles.GetLength(0);i++) {
                for(int j = 0;j<tiles.GetLength(1);j++) {
                    tiles[i, j].CalculateWalls(this);
                }
            }
        }

        public override void Draw(GameTime gt) {
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            sb.Begin();
            for (int j = 0; j < tiles.GetLength(1); j++) {
                for (int i = 0; i < tiles.GetLength(0); i++) {
                    tiles[i, j].Draw(sb);
                }
            }
            sb.End();
        }
    }
}

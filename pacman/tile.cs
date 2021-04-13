using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace pacman {
    class tile : DrawableGameComponent {
        int size = 28;
        int wallID = 0;
        public Vector2 position;
        public int tileID;
        Texture2D tileTexture;
        
        public tile(Game game): base(game) {
            
        }
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
            if(tileID == 0) {
                if(CheckWallNeighbours(map)) {
                    List<tile> neighbours = CheckNeighbours(map);
                    if(neighbours.Count == 3) {
                        Debug.WriteLine(neighbours[0].position.X);
                        
                    }
                    if(neighbours.Count == 2) {

                    }
                } else {
                    this.wallID = 0;
                }
            }
        }

        public List<tile> CheckNeighbours(gameMap map) {
            tile[,] grid = map.tiles;
            int i = (int)this.position.X;
            int j = (int)this.position.Y;
            List<tile> neighbours = new List<tile>();
            if((j - 1) >= 0 && grid[i, j - 1].tileID == 0) {
                neighbours.Add(grid[i, j - 1]);
            }
            if((i + 1) < map.width && grid[i + 1, j].tileID == 0) {
                neighbours.Add(grid[i + 1, j]);
            }
            if((j + 1) < map.height && grid[i, j + 1].tileID == 0) {
                neighbours.Add(grid[i, j + 1]);
            }
            if((i - 1) >= 0 && grid[i - 1, j].tileID == 0) {
                neighbours.Add(grid[i - 1, j]);
            }


            return neighbours;
        }

        public bool CheckWallNeighbours(gameMap map) {
            tile[,] grid = map.tiles;
            int i = (int)this.position.X;
            int j = (int)this.position.Y;
            if ((j - 1) >= 0 && grid[i, j - 1].tileID == 1) {
                return true;
            }
            if ((i + 1) < map.width && grid[i + 1, j].tileID == 1) {
                return true;
            }
            if ((j + 1) < map.height && grid[i, j + 1].tileID == 1) {
                return true;
            }
            if ((i - 1) >= 0 && grid[i - 1, j].tileID == 1) {
                return true;
            }
            return false;
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
            tiles = new tile[this.width, this.height];

           
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
            //Debug.WriteLine(tiles[2, 6].CheckNeighbours(this).Count);
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

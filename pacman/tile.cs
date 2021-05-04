using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace pacman {
    class tile : DrawableGameComponent {
        public int size = 28;
        int wallID = 0;
        public Vector2 position;
        public int tileID;
        Texture2D tileTexture;
        public bool innerCorner = false;
        public Game game;

        public tile(Game game, int i, int j, int tileID, Texture2D spriteMap) : base(game) {
            this.position.X = i;
            this.position.Y = j;
            this.tileID = tileID;
            this.game = game;
        }
        public void Init() {
            //this.position = position;
            if (this.tileID == 1) {
                Texture2D texture = new Texture2D(GraphicsDevice, 1, 1);
                texture.SetData(new Color[] { Color.Black });
                this.tileTexture = texture;
            } else if (this.tileID == 2) {
                Game1.maxScoreCount++;
                Texture2D allTiles = Game.Content.Load<Texture2D>("spriteMap_pacman");
                Rectangle source = new Rectangle(size * 6, 0, size, size);
                tileTexture = new Texture2D(GraphicsDevice, source.Width, source.Height);
                Color[] data = new Color[source.Width * source.Height];
                allTiles.GetData(0, source, data, 0, data.Length);
                tileTexture.SetData(data);
            } else if (this.wallID == 6) {
                Texture2D texture = new Texture2D(GraphicsDevice, 1, 1);
                texture.SetData(new Color[] { Color.Black });
                this.tileTexture = texture;
            } else {
                Texture2D allTiles = Game.Content.Load<Texture2D>("spriteMap_pacman");
                Rectangle source = new Rectangle(size * this.wallID, 0, size, size);
                tileTexture = new Texture2D(GraphicsDevice, source.Width, source.Height);
                Color[] data = new Color[source.Width * source.Height];
                allTiles.GetData(0, source, data, 0, data.Length);
                tileTexture.SetData(data);
            }

        }

        public void CalculateWalls(gameMap map) {
            if (tileID == 0) {
                if (CheckWallNeighbours(map)) {
                    List<tile> neighbours = CheckNeighbours(map);
                    if (neighbours.Count == 3) {
                        if (this.position.Y - neighbours[0].position.Y == 1 && this.position.X - neighbours[1].position.X == -1 && this.position.X - neighbours[2].position.X == 1) {
                            this.wallID = 0;
                        } else if (this.position.Y - neighbours[0].position.Y == 1 && this.position.X - neighbours[1].position.X == -1 && this.position.Y - neighbours[2].position.Y == -1) {
                            this.wallID = 1;
                        } else if (this.position.X - neighbours[0].position.X == -1 && this.position.Y - neighbours[1].position.Y == -1 && this.position.X - neighbours[2].position.X == 1) {
                            this.wallID = 0;
                        } else if (this.position.Y - neighbours[0].position.Y == 1 && this.position.Y - neighbours[1].position.Y == -1 && this.position.X - neighbours[2].position.X == 1) {
                            this.wallID = 1;
                        }
                    } else if (neighbours.Count == 2) {
                        if (this.position.Y - neighbours[0].position.Y == 1 && this.position.X - neighbours[1].position.X == -1) {
                            this.wallID = 4;
                        } else if (this.position.X - neighbours[0].position.X == -1 && this.position.Y - neighbours[1].position.Y == -1) {
                            this.wallID = 5;
                        } else if (this.position.Y - neighbours[0].position.Y == -1 && this.position.X - neighbours[1].position.X == 1) {
                            this.wallID = 2;
                        } else if (this.position.Y - neighbours[0].position.Y == 1 && this.position.X - neighbours[1].position.X == 1) {
                            this.wallID = 3;
                        } else if (this.position.Y - neighbours[0].position.Y == 1 && this.position.Y - neighbours[1].position.Y == -1) {
                            this.wallID = 1;
                        }
                    } else if (neighbours.Count == 4) {
                        this.innerCorner = true;
                    } else {
                        this.wallID = 0;
                    }
                } else {
                    this.wallID = 6;
                }

            }
        }

        public void CalculateInnerCorner(gameMap map) {
            List<tile> neighbours = this.CheckNeighbours(map);
            if (neighbours[0].wallID == 1 && !neighbours[0].innerCorner && neighbours[1].wallID == 0 && !neighbours[1].innerCorner) {
                this.wallID = 4;
            } else if (neighbours[1].wallID == 0 && !neighbours[1].innerCorner && neighbours[2].wallID == 1 && !neighbours[2].innerCorner) {
                this.wallID = 5;
            } else if (neighbours[2].wallID == 1 && !neighbours[2].innerCorner && neighbours[3].wallID == 0 && !neighbours[3].innerCorner) {
                this.wallID = 2;
            } else if (neighbours[3].wallID == 0 && !neighbours[3].innerCorner && neighbours[0].wallID == 1 && !neighbours[0].innerCorner) {
                this.wallID = 3;
            }
        }

        public List<tile> CheckNeighbours(gameMap map) {
            tile[,] grid = map.tiles;
            int i = (int)this.position.X;
            int j = (int)this.position.Y;
            List<tile> neighbours = new List<tile>();
            if ((j - 1) >= 0 && grid[i, j - 1].tileID == 0 && grid[i, j - 1].CheckWallNeighbours(map)) {
                neighbours.Add(grid[i, j - 1]);
            }
            if ((i + 1) < map.width && grid[i + 1, j].tileID == 0 && grid[i + 1, j].CheckWallNeighbours(map)) {
                neighbours.Add(grid[i + 1, j]);
            }
            if ((j + 1) < map.height && grid[i, j + 1].tileID == 0 && grid[i, j + 1].CheckWallNeighbours(map)) {
                neighbours.Add(grid[i, j + 1]);
            }
            if ((i - 1) >= 0 && grid[i - 1, j].tileID == 0 && grid[i - 1, j].CheckWallNeighbours(map)) {
                neighbours.Add(grid[i - 1, j]);
            }


            return neighbours;
        }

        public List<tile> CheckWheyNeighbours(gameMap map) {
            tile[,] grid = map.tiles;
            int i = (int)this.position.X;
            int j = (int)this.position.Y;
            List<tile> neighbours = new List<tile>();
            if ((j - 1) >= 0 && grid[i, j - 1].tileID == 1 || (j - 1) >= 0 && grid[i, j - 1].tileID == 2) {
                neighbours.Add(grid[i, j - 1]);
            }
            if ((i + 1) < map.width && grid[i + 1, j].tileID == 1 || (j - 1) >= 0 && grid[i, j - 1].tileID == 2) {
                neighbours.Add(grid[i + 1, j]);
            }
            if ((j + 1) < map.height && grid[i, j + 1].tileID == 1 || (j - 1) >= 0 && grid[i, j - 1].tileID == 2) {
                neighbours.Add(grid[i, j + 1]);
            }
            if ((i - 1) >= 0 && grid[i - 1, j].tileID == 1 || (j - 1) >= 0 && grid[i, j - 1].tileID == 2) {
                neighbours.Add(grid[i - 1, j]);
            }


            return neighbours;
        }

        public bool CheckWallNeighbours(gameMap map) {
            tile[,] grid = map.tiles;
            int i = (int)this.position.X;
            int j = (int)this.position.Y;
            if ((j - 1) >= 0 && (grid[i, j - 1].tileID == 1 || grid[i, j - 1].tileID == 2)) {
                return true;
            } else if ((j - 1) >= 0 && (i + 1) < map.width && (grid[i + 1, j - 1].tileID == 1 || grid[i + 1, j - 1].tileID == 2)) {
                return true;
            } else if ((i + 1) < map.width && (grid[i + 1, j].tileID == 1 || grid[i + 1, j].tileID == 2)) {
                return true;
            } else if ((i + 1) < map.width && (j + 1) < map.height && (grid[i + 1, j + 1].tileID == 1 || grid[i + 1, j + 1].tileID == 2)) {
                return true;
            } else if ((j + 1) < map.height && (grid[i, j + 1].tileID == 1 || grid[i, j + 1].tileID == 2)) {
                return true;
            } else if ((i - 1) >= 0 && (j + 1) < map.height && (grid[i - 1, j + 1].tileID == 1 || grid[i - 1, j + 1].tileID == 2)) {
                return true;
            } else if ((i - 1) >= 0 && (grid[i - 1, j].tileID == 1 || grid[i - 1, j].tileID == 1)) {
                return true;
            } else if ((i - 1) >= 0 && (j - 1) >= 0 && (grid[i - 1, j - 1].tileID == 1 || grid[i - 1, j - 1].tileID == 2)) {
                return true;
            }
            return false;
        }

        public void LoadTexture() {

        }

        public void Draw(SpriteBatch sb) {
            //RenderTarget2D target = new RenderTarget2D(GraphicsDevice, size, size);
            Rectangle target = new Rectangle((int)Math.Round((position.X * size) * Game1.scaling), (int)Math.Round((position.Y * size) * Game1.scaling), (int)Math.Round(size * Game1.scaling), (int)Math.Round(size * Game1.scaling));
            sb.Draw(tileTexture, target, Color.White);
        }

    }
    class gameMap : DrawableGameComponent {
        public int width, height;
        public tile[,] tiles;
        public Texture2D spriteMap;
        public gameMap(Game game, int width, int height, Texture2D spriteMap) : base(game) {
            this.width = width;
            this.height = height;
            tiles = new tile[this.width, this.height];
            this.spriteMap = spriteMap;


            using (StringReader testSR = new StringReader(Properties.Resources.pacmapText)) {
                for (int j = 0; j < tiles.GetLength(1); j++) {
                    for (int i = 0; i < tiles.GetLength(0); i++) {
                        int symbol = testSR.Read() - 0x30;
                        tile t = new tile(game, i, j, symbol, spriteMap);
                        tiles[i, j] = t;
                    }
                    testSR.Read();
                    testSR.Read();//fixar radbrytningar
                }
            }
        }

        public void InitializeWalls() {
            for (int i = 0; i < tiles.GetLength(0); i++) {
                for (int j = 0; j < tiles.GetLength(1); j++) {
                    tiles[i, j].CalculateWalls(this);

                }
            }
            for (int i = 0; i < tiles.GetLength(0); i++) {
                for (int j = 0; j < tiles.GetLength(1); j++) {
                    if (tiles[i, j].innerCorner) {
                        tiles[i, j].CalculateInnerCorner(this);
                    }
                    tiles[i, j].Init();
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

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
        public tile tileOne;
        public tile tileTwo;
        public tile tileThree;

        public ghost(Game game, int i, int j,int speed):base(game) {

            this.speed = speed;
            this.pos.X = i * 28 + 14;
            this.pos.Y = j * 28 + 14;
            this.dir.X = -1;
            this.dir.Y = 0;
        }

        public void ghostDied() {

        }

        public void Update(gameMap map, pacman torsten) {
            this.currentI = (int)((this.pos.X) / map.tiles[0, 0].size);
            this.currentJ = (int)((this.pos.Y) / map.tiles[0, 0].size);
            var kstate = Keyboard.GetState();
            var neighbour = map.tiles[currentI, currentJ].CheckNeighbours(map);
            List<tile> nextNeighbour = map.tiles[currentI, currentJ].CheckWheyNeighbours(map);

            if ((int)((this.pos.X + (this.dir.X * 14)) / 28) < 0) {
                this.currentI = 27;
                this.pos.X = currentI * 28;
            }
            else if ((int)((this.pos.X + (this.dir.X * 14)) / 28) > 27) {
                this.currentI = 0;
                this.pos.X = currentI * 28;
            }

            if (neighbour.Count == 2) {

                if (map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 1
                    || map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 2) {
                    this.pos.X += this.dir.X * this.speed;
                    this.pos.Y += this.dir.Y * this.speed;
                }

                else {
                    this.pos.X = this.currentI * 28 + 14;
                    this.pos.Y = this.currentJ * 28 + 14;

                    if (map.tiles[this.currentI, this.currentJ + 1].tileID == 0 && (map.tiles[this.currentI + 1, this.currentJ].tileID == 0 || map.tiles[this.currentI - 1, this.currentJ].tileID == 0) && this.dir.X != 0) {
                        this.dir.X = 0;
                        this.dir.Y = -1;
                    }
                    else if (map.tiles[this.currentI, this.currentJ - 1].tileID == 0 && (map.tiles[this.currentI + 1, this.currentJ].tileID == 0 || map.tiles[this.currentI - 1, this.currentJ].tileID == 0) && this.dir.X != 0) {
                        this.dir.X = 0;
                        this.dir.Y = 1;
                    }
                    else if (map.tiles[this.currentI + 1, this.currentJ].tileID == 0 && (map.tiles[this.currentI, this.currentJ - 1].tileID == 0 || map.tiles[this.currentI, this.currentJ + 1].tileID == 0) && this.dir.Y != 0) {
                        this.dir.X = -1;
                        this.dir.Y = 0;
                    }
                    else if (map.tiles[this.currentI - 1, this.currentJ].tileID == 0 && (map.tiles[this.currentI, this.currentJ - 1].tileID == 0 || map.tiles[this.currentI, this.currentJ + 1].tileID == 0) && this.dir.Y != 0) {
                        this.dir.X = 1;
                        this.dir.Y = 0;
                    }

                }



            }
            else if (neighbour.Count == 1 && ((this.pos.X % 28 < 14 + this.speed / 2 && this.pos.X % 28 > 14 - this.speed / 2) && (this.pos.Y % 28 < 14 + this.speed / 2 && this.pos.Y % 28 > 14 - this.speed / 2))) {

                if (this.dir.X == -1) {
                    if (map.tiles[currentI, currentJ + 1].tileID == 0) {
                        this.tileOne = nextNeighbour[0];
                        this.tileTwo = nextNeighbour[2];

                        double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                        double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);

                        if (pathOne > pathTwo) {
                            this.dir.Y = 0;
                            this.dir.X = -1;
                        }
                        else {
                            this.dir.Y = -1;
                            this.dir.X = 0;
                        }
                    }
                    else if (map.tiles[currentI - 1, currentJ].tileID == 0) {
                        this.tileOne = nextNeighbour[0];
                        this.tileTwo = nextNeighbour[2];

                        double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                        double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);

                        if (pathOne > pathTwo) {
                            this.dir.Y = 1;
                            this.dir.X = 0;
                        }
                        else {
                            this.dir.Y = -1;
                            this.dir.X = 0;
                        }
                    }
                    else if (map.tiles[currentI, currentJ - 1].tileID == 0) {
                        this.tileOne = nextNeighbour[1];
                        this.tileTwo = nextNeighbour[2];

                        double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                        double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);

                        if (pathOne > pathTwo) {
                            this.dir.Y = 0;
                            this.dir.X = -1;
                        }
                        else {
                            this.dir.Y = 1;
                            this.dir.X = 0;
                        }
                    }
                }
                else if (this.dir.X == 1) {
                    if (map.tiles[currentI, currentJ + 1].tileID == 0) {
                        this.tileOne = nextNeighbour[0];
                        this.tileTwo = nextNeighbour[1];

                        double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                        double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);

                        if (pathOne > pathTwo) {
                            this.dir.Y = 0;
                            this.dir.X = 1;
                        }
                        else {
                            this.dir.Y = -1;
                            this.dir.X = 0;
                        }
                    }
                    else if (map.tiles[currentI + 1, currentJ].tileID == 0) {
                        this.tileOne = nextNeighbour[0];
                        this.tileTwo = nextNeighbour[1];

                        double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                        double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);

                        if (pathOne > pathTwo) {
                            this.dir.Y = 1;
                            this.dir.X = 0;
                        }
                        else {
                            this.dir.Y = -1;
                            this.dir.X = 0;
                        }
                    }
                    else if (map.tiles[currentI, currentJ - 1].tileID == 0) {
                        this.tileOne = nextNeighbour[0];
                        this.tileTwo = nextNeighbour[1];

                        double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                        double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);

                        if (pathOne > pathTwo) {
                            this.dir.Y = 1;
                            this.dir.X = 0;
                        }
                        else {
                            this.dir.Y = 0;
                            this.dir.X = 1;
                        }
                    }
                }
                else if (this.dir.Y == -1) {
                    if (map.tiles[currentI - 1, currentJ].tileID == 0) {
                        this.tileOne = nextNeighbour[0];
                        this.tileTwo = nextNeighbour[1];

                        double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                        double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);

                        if (pathOne > pathTwo) {
                            this.dir.Y = 0;
                            this.dir.X = 1;
                        }
                        else {
                            this.dir.Y = -1;
                            this.dir.X = 0;
                        }
                    }
                    else if (map.tiles[currentI + 1, currentJ].tileID == 0) {
                        this.tileOne = nextNeighbour[0];
                        this.tileTwo = nextNeighbour[2];

                        double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                        double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);

                        if (pathOne > pathTwo) {
                            this.dir.Y = 0;
                            this.dir.X = -1;
                        }
                        else {
                            this.dir.Y = -1;
                            this.dir.X = 0;
                        }
                    }
                    else if (map.tiles[currentI, currentJ - 1].tileID == 0) {
                        this.tileOne = nextNeighbour[0];
                        this.tileTwo = nextNeighbour[2];

                        double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                        double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);

                        if (pathOne > pathTwo) {
                            this.dir.Y = 0;
                            this.dir.X = -1;
                        }
                        else {
                            this.dir.Y = 0;
                            this.dir.X = 1;
                        }
                    }
                }
                else if (this.dir.Y == 1) {
                    if (map.tiles[currentI - 1, currentJ].tileID == 0) {
                        this.tileOne = nextNeighbour[1];
                        this.tileTwo = nextNeighbour[2];

                        double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                        double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);

                        if (pathOne > pathTwo) {
                            this.dir.Y = 1;
                            this.dir.X = 0;
                        }
                        else {
                            this.dir.Y = 0;
                            this.dir.X = 1;
                        }
                    }
                    else if (map.tiles[currentI + 1, currentJ].tileID == 0) {
                        this.tileOne = nextNeighbour[1];
                        this.tileTwo = nextNeighbour[2];

                        double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                        double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);

                        if (pathOne > pathTwo) {
                            this.dir.Y = 0;
                            this.dir.X = -1;
                        }
                        else {
                            this.dir.Y = 1;
                            this.dir.X = 0;
                        }
                    }
                    else if (map.tiles[currentI, currentJ + 1].tileID == 0) {
                        this.tileOne = nextNeighbour[1];
                        this.tileTwo = nextNeighbour[2];

                        double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                        double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);

                        if (pathOne > pathTwo) {
                            this.dir.Y = 0;
                            this.dir.X = -1;
                        }
                        else {
                            this.dir.Y = 0;
                            this.dir.X = 1;
                        }
                    }
                }

                if (map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 1
                    || map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 2) {
                    this.pos.X += this.dir.X * this.speed;
                    this.pos.Y += this.dir.Y * this.speed;
                }

            } 
            else if (neighbour.Count == 0 && ((this.pos.X % 28 < 14 + this.speed / 2 && this.pos.X % 28 > 14 - this.speed / 2) && (this.pos.Y % 28 < 14 + this.speed / 2 && this.pos.Y % 28 > 14 - this.speed / 2))) {
                if(this.dir.X == -1) {
                    this.tileOne = nextNeighbour[0];
                    this.tileTwo = nextNeighbour[2];
                    this.tileThree = nextNeighbour[3];

                    double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                    double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);
                    double pathThree = pyth((int)this.tileThree.position.X - torsten.currentI, (int)this.tileThree.position.Y - torsten.currentJ);

                    if (pathOne < pathTwo && pathOne < pathThree) {
                        this.dir.X = 0;
                        this.dir.Y = -1;
                    }
                    else if (pathTwo < pathOne && pathTwo < pathThree) {
                        this.dir.X = 0;
                        this.dir.Y = 1;
                    }
                    else {
                        this.dir.X = -1;
                        this.dir.Y = 0;
                    }

                }
                else if (this.dir.X == 1) {
                    this.tileOne = nextNeighbour[0];
                    this.tileTwo = nextNeighbour[1];
                    this.tileThree = nextNeighbour[2];

                    double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                    double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);
                    double pathThree = pyth((int)this.tileThree.position.X - torsten.currentI, (int)this.tileThree.position.Y - torsten.currentJ);

                    if (pathOne < pathTwo && pathOne < pathThree) {
                        this.dir.X = 0;
                        this.dir.Y = -1;
                    }
                    else if (pathTwo < pathOne && pathTwo < pathThree) {
                        this.dir.X = 1;
                        this.dir.Y = 0;
                    }
                    else {
                        this.dir.X = 0;
                        this.dir.Y = 1;
                    }
                }
                else if (this.dir.Y == -1) {
                    this.tileOne = nextNeighbour[1];
                    this.tileTwo = nextNeighbour[2];
                    this.tileThree = nextNeighbour[3];

                    double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                    double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);
                    double pathThree = pyth((int)this.tileThree.position.X - torsten.currentI, (int)this.tileThree.position.Y - torsten.currentJ);

                    if (pathOne < pathTwo && pathOne < pathThree) {
                        this.dir.X = 1;
                        this.dir.Y = 0;
                    }
                    else if (pathTwo < pathOne && pathTwo < pathThree) {
                        this.dir.X = 0;
                        this.dir.Y = -1;
                    }
                    else {
                        this.dir.X = -1;
                        this.dir.Y = 0;
                    }
                }
                else if (this.dir.Y == 1) {
                    this.tileOne = nextNeighbour[1];
                    this.tileTwo = nextNeighbour[2];
                    this.tileThree = nextNeighbour[3];

                    double pathOne = pyth((int)this.tileOne.position.X - torsten.currentI, (int)this.tileOne.position.Y - torsten.currentJ);
                    double pathTwo = pyth((int)this.tileTwo.position.X - torsten.currentI, (int)this.tileTwo.position.Y - torsten.currentJ);
                    double pathThree = pyth((int)this.tileThree.position.X - torsten.currentI, (int)this.tileThree.position.Y - torsten.currentJ);

                    if (pathOne < pathTwo && pathOne < pathThree) {
                        this.dir.X = 1;
                        this.dir.Y = 0;
                    }
                    else if (pathTwo < pathOne && pathTwo < pathThree) {
                        this.dir.X = 0;
                        this.dir.Y = 1;
                    }
                    else {
                        this.dir.X = -1;
                        this.dir.Y = 0;
                    }

                } if (map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 1
                    || map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 2) {
                    this.pos.X += this.dir.X * this.speed;
                    this.pos.Y += this.dir.Y * this.speed;
                }
            } else {
                if (map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 1
                    || map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 2) {
                    this.pos.X += this.dir.X * this.speed;
                    this.pos.Y += this.dir.Y * this.speed;
                }
            }
        }

        public double pyth(int a, int b) {
            double hypo = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
            return hypo;
        }

        public void Draw(GameTime gt) {
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            this.texture = Game.Content.Load<Texture2D>("spöke_1");

            sb.Begin();
            sb.Draw(this.texture, new Rectangle((int)Math.Round((this.pos.X - this.width / 2) * Game1.scaling), (int)Math.Round((this.pos.Y - this.width / 2) * Game1.scaling), (int)Math.Round(this.width * Game1.scaling), (int)Math.Round(this.width * Game1.scaling)), Color.White);
            sb.End();
        }

    }
}

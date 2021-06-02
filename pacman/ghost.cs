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
        public Vector2 homePos;
        public double homeAngle;
        public double homeSpeed;
        public tile tileOne;
        public tile tileTwo;
        public tile tileThree;
        public string mode = "scatter";
        public string ghostVer;
        public Stopwatch stateTimer;
        public int[] ghostStateDuration = new int[7] { 7, 20, 7, 20, 5, 20, 5 };
        public int stateCounter = 0;
        public tile targetTile;
        public Game game;

        public ghost(Game game, int i, int j, int speed, string ghostVer, gameMap map) : base(game) {
            this.speed = speed;
            this.homeSpeed = 1;
            this.pos.X = i * 28 + 14;
            this.pos.Y = j * 28 + 14;
            this.dir.X = -1;
            this.dir.Y = 0;
            this.ghostVer = ghostVer;
            this.game = game;

            this.homePos = new Vector2(392, 490);
            stateTimer = new Stopwatch();
            stateTimer.Start();
            Debug.WriteLine(ghostVer);

            initTexture();
        }

        public void initTexture() {
            if (ghostVer == "blinky") {
                this.texture = Game.Content.Load<Texture2D>("spöke_1");
            } else if (ghostVer == "inky") {
                this.texture = Game.Content.Load<Texture2D>("inky");
            } else if (ghostVer == "pinky") {
                this.texture = Game.Content.Load<Texture2D>("pinky");
            } else if (ghostVer == "clyde") {
                this.texture = Game.Content.Load<Texture2D>("clyde");
            }
        }

        private void stateChange() {
            //Debug.WriteLine(mode);
            stateTimer.Stop();
            stateTimer.Reset();
            stateCounter++;
            if (stateCounter % 2 == 0) {
                mode = "scatter";
            } else {
                mode = "chase";
            }
            stateTimer.Start();
            
        }

        public void ghostDied() {
            mode = "dead";
            homeAngle = Math.Atan((homePos.Y - pos.Y) / (homePos.X - pos.X));
            initTexture();

            if (homePos.X - pos.X > 0) {
                this.dir.X = 1;
                this.dir.Y = 1;
            } else {
                this.dir.X = -1;
                this.dir.Y = -1;
            }
        }

        public void updateDir(gameMap map, pacman torsten) {
            var neighbour = map.tiles[currentI, currentJ].CheckNeighbours(map);
            List<tile> nextNeighbour = map.tiles[currentI, currentJ].CheckWheyNeighbours(map);

            if(ghostVer == "blinky") {
                targetTile = new tile(game, torsten.currentI, torsten.currentJ, 1, texture);
            } else if(ghostVer == "inky") {
                
            } else if(ghostVer == "pinky") {
                if(torsten.dir.X == 1) {
                    targetTile = new tile(game, torsten.currentI + 4, torsten.currentJ, 1, texture);
                } else if(torsten.dir.X == -1) {
                    targetTile = new tile(game, torsten.currentI - 4, torsten.currentJ, 1, texture);
                } else if(torsten.dir.Y == 1) {
                    targetTile = new tile(game, torsten.currentI, torsten.currentJ + 4, 1, texture);
                } else if(torsten.dir.Y == -1) {
                    targetTile = new tile(game, torsten.currentI, torsten.currentJ - 4, 1, texture);
                }
            } else if(ghostVer == "clyde") {
                double pacDist = pyth((int)this.tileOne.position.X - (int)torsten.currentI, (int)this.tileOne.position.Y - (int)torsten.currentJ);

                if(pacDist >= 6) {
                    targetTile = new tile(game, torsten.currentI, torsten.currentJ, 1, texture);
                } else {
                    targetTile = new tile(game, 1, 30, 1, texture);
                }
            }



            if(mode == "scatter") {
                if(ghostVer == "blinky") {
                    targetTile = new tile(game, 26, 1, 1, texture);
                } else if(ghostVer == "inky") {
                    targetTile = new tile(game, 2, 1, 1, texture);
                } else if(ghostVer == "pinky") {
                    targetTile = new tile(game, 28, 30, 1, texture);
                } else if(ghostVer == "clyde") {
                    targetTile = new tile(game, 1, 30, 1, texture);
                }
            }



            if (neighbour.Count == 2) {
                if (map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID != 1) {
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
            else if (neighbour.Count == 1 && ((this.pos.X % 28 < 14 + Math.Ceiling((double)this.speed / 2) && this.pos.X % 28 > 14 - Math.Ceiling((double)this.speed / 2)) && (this.pos.Y % 28 < 14 + Math.Ceiling((double)this.speed / 2) && this.pos.Y % 28 > 14 - Math.Ceiling((double)speed / 2)))) {
                if (this.dir.X == -1) {
                    if (map.tiles[currentI, currentJ + 1].tileID == 0) {
                        this.tileOne = nextNeighbour[0];
                        this.tileTwo = nextNeighbour[2];

                        double pathOne;
                        double pathTwo;

                        if (this.mode != "frightened") {
                            pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        }
                        else {
                            pathOne = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        }

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

                        double pathOne;
                        double pathTwo;

                        if (this.mode != "frightened") {
                            pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        }
                        else {
                            pathOne = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        }

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

                        double pathOne;
                        double pathTwo;

                        if (this.mode != "frightened") {
                            pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        }
                        else {
                            pathOne = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        }

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

                        double pathOne;
                        double pathTwo;

                        if (this.mode != "frightened") {
                            pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        }
                        else {
                            pathOne = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        }

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

                        double pathOne;
                        double pathTwo;

                        if (this.mode != "frightened") {
                            pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        }
                        else {
                            pathOne = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        }

                        Debug.WriteLine(pathOne.ToString() + ", " + pathTwo.ToString());

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

                        double pathOne;
                        double pathTwo;

                        if (this.mode != "frightened") {
                            pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        }
                        else {
                            pathOne = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        }

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

                        double pathOne;
                        double pathTwo;

                        if (this.mode != "frightened") {
                            pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        }
                        else {
                            pathOne = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        }

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

                        double pathOne;
                        double pathTwo;

                        if (this.mode != "frightened") {
                            pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        }
                        else {
                            pathOne = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        }

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

                        double pathOne;
                        double pathTwo;

                        if (this.mode != "frightened") {
                            pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        }
                        else {
                            pathOne = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        }

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

                        double pathOne;
                        double pathTwo;

                        if (this.mode != "frightened") {
                            pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        }
                        else {
                            pathOne = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        }

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

                        double pathOne;
                        double pathTwo;

                        if (this.mode != "frightened") {
                            pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        }
                        else {
                            pathOne = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        }

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

                        double pathOne;
                        double pathTwo;

                        if (this.mode != "frightened") {
                            pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        }
                        else {
                            pathOne = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                            pathTwo = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        }

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
            }
            else if (neighbour.Count == 0 && ((this.pos.X % 28 < 14 + Math.Ceiling((double)this.speed / 2) && this.pos.X % 28 > 14 - Math.Ceiling((double)this.speed / 2)) && (this.pos.Y % 28 < 14 + Math.Ceiling((double)this.speed / 2) && this.pos.Y % 28 > 14 - Math.Ceiling((double)this.speed / 2)))) {
                if (this.dir.X == -1) {
                    this.tileOne = nextNeighbour[0];
                    this.tileTwo = nextNeighbour[2];
                    this.tileThree = nextNeighbour[3];

                    double pathOne;
                    double pathTwo;
                    double pathThree;

                    if (this.mode != "frightened") {
                        pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        pathThree = pyth((int)this.tileThree.position.X - (int)targetTile.position.X, (int)this.tileThree.position.Y - (int)targetTile.position.Y);
                    }
                    else {
                        pathThree = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        pathOne = pyth((int)this.tileThree.position.X - (int)targetTile.position.X, (int)this.tileThree.position.Y - (int)targetTile.position.Y);
                    }


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

                    double pathOne;
                    double pathTwo;
                    double pathThree;

                    if (this.mode != "frightened") {
                        pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        pathThree = pyth((int)this.tileThree.position.X - (int)targetTile.position.X, (int)this.tileThree.position.Y - (int)targetTile.position.Y);
                    }
                    else {
                        pathThree = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        pathOne = pyth((int)this.tileThree.position.X - (int)targetTile.position.X, (int)this.tileThree.position.Y - (int)targetTile.position.Y);
                    }

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
                    this.tileOne = nextNeighbour[0];
                    this.tileTwo = nextNeighbour[1];
                    this.tileThree = nextNeighbour[3];

                    double pathOne;
                    double pathTwo;
                    double pathThree;

                    if (this.mode != "frightened") {
                        pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        pathThree = pyth((int)this.tileThree.position.X - (int)targetTile.position.X, (int)this.tileThree.position.Y - (int)targetTile.position.Y);
                    }
                    else {
                        pathThree = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        pathOne = pyth((int)this.tileThree.position.X - (int)targetTile.position.X, (int)this.tileThree.position.Y - (int)targetTile.position.Y);
                    }

                    if (pathOne < pathTwo && pathOne < pathThree) {
                        this.dir.X = 0;
                        this.dir.Y = -1;
                    }
                    else if (pathTwo < pathOne && pathTwo < pathThree) {
                        this.dir.X = 1;
                        this.dir.Y = 0;
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

                    double pathOne;
                    double pathTwo;
                    double pathThree;

                    if (this.mode != "frightened") {
                        pathOne = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        pathThree = pyth((int)this.tileThree.position.X - (int)targetTile.position.X, (int)this.tileThree.position.Y - (int)targetTile.position.Y);
                    }
                    else {
                        pathThree = pyth((int)this.tileOne.position.X - (int)targetTile.position.X, (int)this.tileOne.position.Y - (int)targetTile.position.Y);
                        pathTwo = pyth((int)this.tileTwo.position.X - (int)targetTile.position.X, (int)this.tileTwo.position.Y - (int)targetTile.position.Y);
                        pathOne = pyth((int)this.tileThree.position.X - (int)targetTile.position.X, (int)this.tileThree.position.Y - (int)targetTile.position.Y);
                    }

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

                }
            }
        }

        public void Update(gameMap map, pacman torsten) {

            if(stateCounter < ghostStateDuration.Length && stateTimer.ElapsedMilliseconds >= ghostStateDuration[stateCounter] * 1000 && mode != "dead" && mode != "reviving") {
                stateChange();
            } else if(stateTimer.IsRunning && stateCounter >= ghostStateDuration.Length) {
                stateTimer.Stop();
            }

            if (mode == "chase" || mode == "frightened") {
                this.currentI = (int)((this.pos.X) / map.tiles[0, 0].size);
                this.currentJ = (int)((this.pos.Y) / map.tiles[0, 0].size);

                if ((int)((this.pos.X + (this.dir.X * 14)) / 28) < 0) {
                    this.currentI = 27;
                    this.pos.X = currentI * 28;
                }
                else if ((int)((this.pos.X + (this.dir.X * 14)) / 28) > 27) {
                    this.currentI = 0;
                    this.pos.X = currentI * 28;
                }


                updateDir(map, torsten);
                //Debug.WriteLine(targetTile.position.X + ", " + targetTile.position.Y);
                if (map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 1
                    || map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 2) {
                    this.pos.X += this.dir.X * this.speed;
                    this.pos.Y += this.dir.Y * this.speed;
                }
            }
            else if (mode == "scatter") {
                
                this.currentI = (int)((this.pos.X) / map.tiles[0, 0].size);
                this.currentJ = (int)((this.pos.Y) / map.tiles[0, 0].size);

                if ((int)((this.pos.X + (this.dir.X * 14)) / 28) < 0) {
                    this.currentI = 27;
                    this.pos.X = currentI * 28;
                }
                else if ((int)((this.pos.X + (this.dir.X * 14)) / 28) > 27) {
                    this.currentI = 0;
                    this.pos.X = currentI * 28;
                }

                updateDir(map, torsten);

                //updateScatter(map, torsten);
                //Debug.WriteLine(targetTile.position.X + ", " + targetTile.position.Y);
                if (map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 1
                || map.tiles[(int)((this.pos.X + (this.dir.X * 14)) / 28), (int)((this.pos.Y + this.dir.Y * 14) / 28)].tileID == 2) {
                    this.pos.X += this.dir.X * this.speed;
                    this.pos.Y += this.dir.Y * this.speed;

                }

            } else if (mode == "dead") {
                this.currentI = (int)((this.pos.X) / map.tiles[0, 0].size);
                this.currentJ = (int)((this.pos.Y) / map.tiles[0, 0].size);

                this.pos.X += (float)Math.Cos(homeAngle) * (float)homeSpeed * this.dir.X;
                this.pos.Y += (float)Math.Sin(homeAngle) * (float)homeSpeed * this.dir.Y;

                if (this.homePos.X - 1 < this.pos.X && this.homePos.X + 1 > this.pos.X && this.homePos.Y - 1 < this.pos.Y && this.homePos.Y + 1 > this.pos.Y) {
                    this.mode = "reviving";
                    Game1.rageMode = false;
                 }
            } else if (mode == "reviving") {
                this.pos.Y -= 1;

                this.currentI = (int)((this.pos.X) / map.tiles[0, 0].size);
                this.currentJ = (int)((this.pos.Y) / map.tiles[0, 0].size);
                if(map.tiles[this.currentI, this.currentJ].tileID == 1 && this.pos.Y % 28 < 14 + Math.Ceiling((double)this.speed / 2) && this.pos.Y % 28 > 14 - Math.Ceiling((double)this.speed / 2)) {
                    this.mode = "chase";
                    this.dir.X = -1;
                    this.dir.Y = 0;
                    this.speed = 2;
                    this.stateTimer.Start();
                }
            }

        }

        public double pyth(int a, int b) {
            double hypo = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
            return hypo;
        }

        public void Draw(GameTime gt) {
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            

            sb.Begin();
            sb.Draw(this.texture, new Rectangle((int)Math.Round((this.pos.X - this.width / 2) * Game1.scaling), (int)Math.Round((this.pos.Y - this.width / 2) * Game1.scaling), (int)Math.Round(this.width * Game1.scaling), (int)Math.Round(this.width * Game1.scaling)), Color.White);
            sb.End();
        }

    }
}

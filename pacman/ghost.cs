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
        public gameMap map;
        public ghost(Game game, int i, int j,int speed, pacman torsten, gameMap map):base(game) {

            //tile1 Y - pacman Y = katet1[1]
            //tile2 Y - pacman Y = katet2[1]
            //tile1 X - pacman X = katet1[2]
            //tile2 X - pacman X = katet2[2]

            //compare hypotenuse choose tile.
            //i kolumn
            //j kolumn

            this.speed = speed;
            this.pos.X = i * 28 + 14;
            this.pos.Y = j * 28 + 14;
            this.dir.X = 0;
            this.dir.Y = 0;
        }

        public void update() {

            this.currentI = (int)((this.pos.X) / map.tiles[0, 0].size);
            this.currentJ = (int)((this.pos.Y) / map.tiles[0, 0].size);

            map.tiles[currentI, currentJ].CheckNeighbours(map);

            

            

        }
    }
}

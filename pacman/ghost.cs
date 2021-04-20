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
        public ghost(Game game, int i, int j,int speed):base(game) {

            //tile1 Y - pacman Y = katet1[1]
            //tile2 Y - pacman Y = katet2[1]
            //tile1 X - pacman X = katet1[2]
            //tile2 X - pacman X = katet2[2]

            //compare hypotenuse choose tile.

            this.speed = speed;

        }

    }
}

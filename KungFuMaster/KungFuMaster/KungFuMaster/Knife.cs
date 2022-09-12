using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KungFuMaster
{
    class Knife
    {
        public Rectangle rect;
        public bool isRight;

        public Knife(Rectangle rect, bool isRight)
        {
            this.rect = rect;
            this.isRight = isRight;
        }
    }
}

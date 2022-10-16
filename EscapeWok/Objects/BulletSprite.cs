using EscapeWok.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeWok.Objects
{
    internal class BulletSprite:BaseGameObject
    {
        private const float BULLET_SPEED = 8.0f;

        public BulletSprite(Texture2D texture)
        {
            _texture = texture;
        }        

        public void MoveUp()
        {
            _position = new Vector2(Position.X, Position.Y - BULLET_SPEED);
        }
    }
}

using EscapeWok.Objects.Base;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeWok.Objects
{
    internal class SplashImage:BaseGameObject
    {
        public SplashImage(Texture2D texture)
        {
            _texture = texture;            
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle(0, 0, 1280, 720), new Rectangle(0, 0, _texture.Width, _texture.Height), Color.White);
            //TODO: el 1280 x 720 deberia ser un enum? o algo asi global no el clase mainGame
            //base.Render(spriteBatch);
        }
    }
}

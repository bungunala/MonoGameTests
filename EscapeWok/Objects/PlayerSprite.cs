using EscapeWok.Objects.Base;
using Microsoft.Xna.Framework.Graphics;

namespace EscapeWok.Objects
{
    internal class PlayerSprite : BaseGameObject
    {
        public PlayerSprite(Texture2D texture)
        { 
            _texture =  texture;
        }
    }
}

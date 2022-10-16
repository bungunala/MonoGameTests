using EscapeWok.Enum;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace EscapeWok.Objects.Base
{
    public class BaseGameObject
    {
        protected Texture2D _texture;
        protected Vector2 _position;
        public int zIndex;

        public int Width { get { return _texture.Width; } }
        public int Height { get { return _texture.Height; } }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public virtual void OnNotify(Events eventType){ }

        public virtual void Render(SpriteBatch spriteBatch) {
            //TODO: Call Draw
            spriteBatch.Draw(_texture, Position, Color.White);
        }

    }
}
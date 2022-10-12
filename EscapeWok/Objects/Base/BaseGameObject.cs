using EscapeWok.Enum;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace EscapeWok.Objects.Base
{
    public class BaseGameObject
    {
        protected Texture2D _texture;
        private Vector2 _position;
        public int zIndex;

        public virtual void OnNotify(Events eventType){ }

        public void Render(SpriteBatch spriteBatch) {
            //TODO: Call Draw
            spriteBatch.Draw(_texture, Vector2.One, Color.White);
        }

    }
}
using EscapeWok.Enum;
using EscapeWok.Objects;
using EscapeWok.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace EscapeWok.States
{
    internal class GameplayState : BaseGameState
    {
        private const string PlayerFighter = "fighter";
        private const string BackgroundTexture = "Barren";

        public override void HandleInput()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                NotifyEvent(Events.GAME_QUIT);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            //throw new System.NotImplementedException();
            AddGameObject(new SplashImage(LoadTexture(@"gfx/" + BackgroundTexture)));
            AddGameObject(new PlayerSprite(LoadTexture(@"gfx/"+PlayerFighter)));            
        }

    }
}

using EscapeWok.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace EscapeWok.States
{
    internal class GameplayState : BaseGameState
    {
        public override void HandleInput()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                SwitchState(new GameplayState());
        }

        public override void LoadContent(ContentManager contentManager)
        {
            //throw new System.NotImplementedException();
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            contentManager.Unload();
        }
    }
}

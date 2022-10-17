using EscapeWok.Engine.Input;

namespace EscapeWok.States.Gameplay
{
    internal class GameplayInputCommand : BaseInputCommand
    {
        public class GameExit : GameplayInputCommand { };
        public class PlayerMoveLeft : GameplayInputCommand { };
        public class PlayerMoveRight : GameplayInputCommand { };
        public class PlayerShoots : GameplayInputCommand { };
    }
}

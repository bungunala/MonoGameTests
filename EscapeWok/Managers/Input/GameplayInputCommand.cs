using EscapeWok.Managers.Input.Base;

namespace EscapeWok.Managers.Input
{
    internal class GameplayInputCommand : BaseInputCommand
    {
        public class GameExit : GameplayInputCommand { };
        public class PlayerMoveLeft : GameplayInputCommand { };
        public class PlayerMoveRight : GameplayInputCommand { };
        public class PlayerShoots : GameplayInputCommand { };
    }
}

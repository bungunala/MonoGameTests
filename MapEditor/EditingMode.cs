using Microsoft.Xna.Framework.Input;

namespace MapEditor
{
    enum EditingMode
    {
        None,
        Path,
        Scripts
    }

    public static class EditingModeHelper
    {
        public static bool isInvalidKey(Keys key)
        {
            if (
                (key != Keys.A)
                && (key != Keys.B)
                && (key != Keys.C)
                && (key != Keys.D)
                && (key != Keys.E)
                && (key != Keys.F)
                && (key != Keys.G)
                && (key != Keys.H)
                && (key != Keys.I)
                && (key != Keys.J)
                && (key != Keys.K)
                && (key != Keys.L)
                && (key != Keys.M)
                && (key != Keys.N)
                && (key != Keys.O)
                && (key != Keys.P)
                && (key != Keys.Q)
                && (key != Keys.R)
                && (key != Keys.S)
                && (key != Keys.T)
                && (key != Keys.U)
                && (key != Keys.V)
                && (key != Keys.W)
                && (key != Keys.X)
                && (key != Keys.Y)
                && (key != Keys.Z)
                && (key != Keys.D0)
                && (key != Keys.D1)
                && (key != Keys.D2)
                && (key != Keys.D3)
                && (key != Keys.D4)
                && (key != Keys.D5)
                && (key != Keys.D6)
                && (key != Keys.D7)
                && (key != Keys.D8)
                && (key != Keys.D9)
                && (key != Keys.NumPad0)
                && (key != Keys.NumPad1)
                && (key != Keys.NumPad2)
                && (key != Keys.NumPad3)
                && (key != Keys.NumPad4)
                && (key != Keys.NumPad5)
                && (key != Keys.NumPad6)
                && (key != Keys.NumPad7)
                && (key != Keys.NumPad8)
                && (key != Keys.NumPad9)

                && (key != Keys.Enter)
                && (key != Keys.Back)
                && (key != Keys.Space)

                )
                return true;
            else
                return false;
        }

    }
}

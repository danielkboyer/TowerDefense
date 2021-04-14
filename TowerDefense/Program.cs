using System;

namespace TowerDefense
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameStateDemo())
                game.Run();
        }
    }
}

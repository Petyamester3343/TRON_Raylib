namespace TRON_RayLib
{
    internal class Launcher
    {
        public static void LaunchVS_AI()
        {
            Thread gt = new(TRON_AI.StartGame);
            gt.Start();
        }

        public static void LaunchVS_Local()
        {
            Thread gt = new(TRON_Local.StartGame);
            gt.Start();
        }

        public static void LaunchVS_Online(string? ip = null)
        {
            Thread gt = new(() =>
            {
                TRON_Online onlineGame = new();
                if (string.IsNullOrEmpty(ip))
                {
                    onlineGame.StartServer();
                }
                else
                {
                    onlineGame.Connect(ip);
                }
            });
            gt.Start();
        }
    }
}

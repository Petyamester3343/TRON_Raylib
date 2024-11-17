using Raylib_cs;
using Color = Raylib_cs.Color;
using System.Numerics;

namespace TRON_RayLib
{
    internal class TRON_OnlineGame
    {
        Vector2 p1Pos, p2Pos;
        List<Vector2> p1Trail, p2Trail;

        bool gameRun = true, gameOver = false;
        float speed = 10.0f, moveDelay = 0.01f, moveTimer = 0.0f;

        public enum Dir { N, S, W, E }
        Dir p1Dir, p2Dir;

        TRON_Online network_mng;

        public void StartGame(bool isServer, string ip = "127.0.0.1")
        {
            Raylib.InitWindow(1000, 1000, "TRON - VS ONLINE");
            Raylib.SetTargetFPS(60);

            network_mng = new();
            p1Trail = [];
            p2Trail = [];

            if (isServer)
            {
                network_mng.StartServer();
            }
            else
            {
                network_mng.Connect(ip);
            }

            while (!Raylib.WindowShouldClose() && gameRun)
            {
                moveTimer += Raylib.GetFrameTime();

                if (moveTimer >= moveDelay)
                {
                    if (isServer)
                    {
                        if (Raylib.IsKeyPressed(KeyboardKey.A) && p1Dir != Dir.E) p1Dir = Dir.W;
                        if (Raylib.IsKeyPressed(KeyboardKey.D) && p1Dir != Dir.W) p1Dir = Dir.E;
                        if (Raylib.IsKeyPressed(KeyboardKey.W) && p1Dir != Dir.S) p1Dir = Dir.N;
                        if (Raylib.IsKeyPressed(KeyboardKey.S) && p1Dir != Dir.N) p1Dir = Dir.S;

                        Vector2 newP1Pos = MovePlayer(p1Pos, p1Dir, speed);
                        p1Pos = newP1Pos;

                        network_mng.SendData(p1Pos);

                        p2Pos = network_mng.RecieveData();
                    }
                    else
                    {
                        p1Pos = network_mng.RecieveData();

                        if (Raylib.IsKeyPressed(KeyboardKey.Left) && p1Dir != Dir.E) p1Dir = Dir.W;
                        if (Raylib.IsKeyPressed(KeyboardKey.Right) && p1Dir != Dir.W) p1Dir = Dir.E;
                        if (Raylib.IsKeyPressed(KeyboardKey.Up) && p1Dir != Dir.S) p1Dir = Dir.N;
                        if (Raylib.IsKeyPressed(KeyboardKey.Down) && p1Dir != Dir.N) p1Dir = Dir.S;

                        Vector2 newP2Pos = MovePlayer(p2Pos, p2Dir, speed);
                        p2Pos = newP2Pos;

                        network_mng.SendData(p2Pos);
                    }

                    p1Trail.Add(p1Pos);
                    p2Trail.Add(p2Pos);

                    moveTimer = 0.0f;
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.DrawRectangleV(p1Pos, new(10, 10), Color.Blue);
                Raylib.DrawRectangleV(p2Pos, new(10, 10), Color.Red);

                foreach (var p in p1Trail)
                {
                    Raylib.DrawRectangleV(p, new(10, 10), Color.SkyBlue);
                }

                foreach (var p in p2Trail)
                {
                    Raylib.DrawRectangleV(p, new(10, 10), Color.Orange);
                }

                Raylib.EndDrawing();
            }

            network_mng.Disconnect();
            Raylib.CloseWindow();

            Raylib.CloseWindow();
            MainMenuForm obj = new();
            obj.Show();
        }

        static Vector2 MovePlayer(Vector2 pos, Dir dir, float v) => dir switch
        {
            Dir.N => new(pos.X, pos.Y - v), // Move up
            Dir.S => new(pos.X, pos.Y + v), // Move down
            Dir.W => new(pos.X - v, pos.Y), // Move left
            Dir.E => new(pos.X + v, pos.Y), // Move right
            _ => pos
        };
    }
}

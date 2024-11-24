using Raylib_cs;
using Color = Raylib_cs.Color;
using System.Numerics;

namespace TRON_RayLib
{
    internal class TRON_OnlineGame
    {
        Vector2 pPos, opPos;
        List<Vector2>? p1Trail, p2Trail;

        static int scoreBarH;

        bool gameRun, gameOver, isNameEntered;
        readonly float speed = 10.0f, moveDelay = 0.01f;
        float moveTimer = 0.0f;

        static string pName = "", opName = "";

        public enum Dir { N, S, W, E }

        Dir p1Dir, p2Dir;

        public required TRON_Online network_mng;

        int pScore, opScore;

        public void StartGame(bool isServer, string ip = "127.0.0.1")
        {
            Raylib.InitWindow(1000, 1000, "PHANTASMATRON - VS ONLINE");
            Raylib.SetTargetFPS(60);

            gameOver = false;
            gameRun = true;
            scoreBarH = 60;
            pPos = new(400, 300);
            opPos = new(200, 300);

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

            while (!Raylib.WindowShouldClose())
            {
                if (!isNameEntered)
                {
                    HandleNameInput(ref pName, ref isNameEntered);
                }
                else
                {
                    if (gameOver)
                    {
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Color.Black);

                        // Display game over message
                        Raylib.DrawText("GAME OVER!", 300, 400, 40, Color.Red);
                        Raylib.DrawText("Press R to restart or ESC to exit.", 600, 450, 20, Color.White);

                        Raylib.EndDrawing();

                        if (Raylib.IsKeyPressed(KeyboardKey.R))
                        {
                            RestartGame();
                            gameOver = false;
                        }
                        else if (Raylib.IsKeyPressed(KeyboardKey.Escape))
                        {
                            SaveScores(pName, pScore, opName, opScore);
                            break;
                        }
                    }
                    else
                    {
                        RunGame(isServer, ref gameOver);
                    }
                }

            }
        }

        private void SaveScores(string pName, int pScore, string opName, int opScore)
        {
            HiScoreDBHelper db_helper = new();
            db_helper.AddScore(pName, pScore, opName, opScore);
        }

        private void RestartGame()
        {
            pPos = new(200, 300);
            opPos = new(400, 300);
            p1Dir = p2Dir = Dir.N;
            p1Trail.Clear();
            p2Trail.Clear();
            gameRun = true;
        }

        public void RunGame(bool isServer, ref bool gameOver)
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

                    Vector2 newP1Pos = MovePlayer(pPos, p1Dir, speed);
                    pPos = newP1Pos;

                    network_mng.SendData(pPos);

                    opPos = network_mng.RecieveData();
                }
                else
                {
                    opPos = network_mng.RecieveData();

                    if (Raylib.IsKeyPressed(KeyboardKey.Left) && p1Dir != Dir.E) p1Dir = Dir.W;
                    if (Raylib.IsKeyPressed(KeyboardKey.Right) && p1Dir != Dir.W) p1Dir = Dir.E;
                    if (Raylib.IsKeyPressed(KeyboardKey.Up) && p1Dir != Dir.S) p1Dir = Dir.N;
                    if (Raylib.IsKeyPressed(KeyboardKey.Down) && p1Dir != Dir.N) p1Dir = Dir.S;

                    Vector2 newP2Pos = MovePlayer(opPos, p2Dir, speed);
                    opPos = newP2Pos;

                    network_mng.SendData(opPos);
                }

                p1Trail.Add(pPos);
                p2Trail.Add(opPos);

                moveTimer = 0.0f;
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawRectangleV(pPos, new(10, 10), Color.Blue);
            Raylib.DrawRectangleV(opPos, new(10, 10), Color.Red);

            foreach (var p in p1Trail)
            {
                Raylib.DrawRectangleV(p, new(10, 10), Color.SkyBlue);
            }

            foreach (var p in p2Trail)
            {
                Raylib.DrawRectangleV(p, new(10, 10), Color.Orange);
            }

            Raylib.EndDrawing();


            network_mng.Disconnect();
            Raylib.CloseWindow();

            Raylib.CloseWindow();
            MainMenuForm obj = new();
            obj.Show();
        }

        private void HandleNameInput(ref string pName, ref bool isNameEntered)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawText("Enter your name:", 150, 250, 25, Color.White);
            Raylib.DrawText(pName, 200, 300, 25, Color.SkyBlue);

            int key = Raylib.GetCharPressed();

            while (key > 0)
            {
                if ((key >= 32) && (key <= 125) && pName.Length < 12)
                {
                    pName += (char)key;
                }
                key = Raylib.GetCharPressed();
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Backspace) && pName.Length > 0)
            {
                pName = pName[..^1];
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Enter))
            {
                isNameEntered = true;
            }

            Raylib.EndDrawing();
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

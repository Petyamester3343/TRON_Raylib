using Raylib_cs;
using Color = Raylib_cs.Color;
using System.Numerics;
using System.Xml.Linq;

namespace TRON_RayLib
{
    internal class TRON_AI
    {
        static int w, h;
        static int scoreBarH;
        static Vector2 pPos, aiPos;
        static List<Vector2> pTrail, aiTrail;
        static int pScore = 0, aiScore = 0;
        static float speed = 10.0f;

        static float moveDelay, moveTimer;

        public enum Dir { N, S, W, E }
        static Dir pDir, aiDir;

        static bool gameRun, gameOver, isNameEntered;

        static string pName = "", aiName = "SkyNet";

        public static void StartGame()
        {
            w = h = 1000;
            scoreBarH = 60;
            Raylib.InitWindow(w, h, "PHANTASMATRON - VS AI");
            Raylib.SetTargetFPS(60);

            gameOver = false;
            gameRun = true;
            isNameEntered = false;

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
                        Raylib.DrawText("GAME OVER!", w / 10 * 3, h / 5 + 200, 40, Color.Red);
                        Raylib.DrawText("Press R to restart or ESC to exit.", w / 5 * 3, h / 5 + 250, 20, Color.White);

                        Raylib.EndDrawing();

                        if (Raylib.IsKeyPressed(KeyboardKey.R))
                        {
                            RestartGame();
                            gameOver = false;
                        }
                        else if (Raylib.IsKeyPressed(KeyboardKey.Escape))
                        {
                            SaveScores(pName, pScore, aiName, aiScore);
                            break;
                        }
                    }
                    else
                    {
                        RunGame(ref gameOver);
                    }
                }
            }

            Raylib.CloseWindow();
            MainMenuForm obj = new();
            obj.Show();
        }

        static void HandleNameInput(ref string pName, ref bool isNameEntered)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawText("Enter your name:", w / 10 + 50, h / 5 + 50, 25, Color.White);
            Raylib.DrawText(pName, w / 10 + 100, h / 5 + 100, 25, Color.SkyBlue);

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

        static void RunGame(ref bool gameOver)
        {
            pPos = new(400, 300);
            aiPos = new(200, 300);
            pTrail = [];
            aiTrail = [];

            pDir = aiDir = Dir.N;

            moveDelay = 0.01f;
            moveTimer = 0.0f;
            gameRun = true;

            while (!Raylib.WindowShouldClose() && gameRun)
            {
                moveTimer += Raylib.GetFrameTime();

                if (moveTimer >= moveDelay)
                {
                    if (Raylib.IsKeyPressed(KeyboardKey.A) && pDir != Dir.E) pDir = Dir.W;
                    if (Raylib.IsKeyPressed(KeyboardKey.D) && pDir != Dir.W) pDir = Dir.E;
                    if (Raylib.IsKeyPressed(KeyboardKey.W) && pDir != Dir.S) pDir = Dir.N;
                    if (Raylib.IsKeyPressed(KeyboardKey.S) && pDir != Dir.N) pDir = Dir.S;

                    CalcAIMove(ref aiPos, ref aiDir, aiTrail, pTrail, pPos);

                    Vector2 newPPos = Move(pPos, pDir, speed);
                    Vector2 newAIPos = Move(aiPos, aiDir, speed);

                    if (CheckCollision(newPPos, pTrail) || CheckCollision(newPPos, aiTrail) || IsOutOfBounds(pPos))
                    {
                        aiScore++;
                        gameOver = true;
                        break;
                    }

                    if (CheckCollision(newAIPos, aiTrail) || CheckCollision(newAIPos, pTrail) || IsOutOfBounds(aiPos))
                    {
                        pScore++;
                        gameOver = true;
                        break;
                    }

                    pTrail.Add(pPos);
                    aiTrail.Add(aiPos);

                    pPos = newPPos;
                    aiPos = newAIPos;

                    moveTimer = 0.0f;
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.DrawRectangle(0, 0, w, scoreBarH, Color.DarkGray);
                Raylib.DrawText($"{pName}", 20, 20, 20, Color.SkyBlue);
                Raylib.DrawText($"{aiName}", w - 100, 20, 20, Color.Orange);

                foreach (var p in pTrail)
                {
                    Raylib.DrawRectangleV(p, new(10, 10), Color.SkyBlue);
                }

                foreach (var p in aiTrail)
                {
                    Raylib.DrawRectangleV(p, new(10, 10), Color.Orange);
                }

                Raylib.EndDrawing();
            }
        }

        static void CalcAIMove(ref Vector2 aiPos, ref Dir aiDir, List<Vector2> aiTrail, List<Vector2> pTrail, Vector2 pPos)
        {
            if (IsObstacleAhead(aiPos, aiDir, aiTrail, pTrail))
            {
                aiDir = ChooseNewDir(aiPos, aiTrail, pTrail);
            }
        }

        static bool IsObstacleAhead(Vector2 pos, Dir dir, List<Vector2> aiTrail, List<Vector2> pTrail)
        {
            Vector2 next = Move(pos, dir, 10.0f);

            return IsOutOfBounds(next) || CheckCollision(next, aiTrail) || CheckCollision(next, pTrail);
        }

        static Dir ChooseNewDir(Vector2 aiPos, List<Vector2> aiTrail, List<Vector2> pTrail)
        {
            Dictionary<Dir, int> openSpaces = new()
            {
                { Dir.N, CountOpenSpaces(Move(aiPos, Dir.N, 10.0f), aiTrail, pTrail) },
                { Dir.S, CountOpenSpaces(Move(aiPos, Dir.S, 10.0f), aiTrail, pTrail) },
                { Dir.W, CountOpenSpaces(Move(aiPos, Dir.W, 10.0f), aiTrail, pTrail) },
                { Dir.E, CountOpenSpaces(Move(aiPos, Dir.E, 10.0f), aiTrail, pTrail) }
            };

            Dir best = Dir.N;
            int max = openSpaces[Dir.N];

            foreach (var d in openSpaces)
            {
                if (d.Value > max)
                {
                    best = d.Key;
                    max = d.Value;
                }
            }

            return best;
        }

        static int CountOpenSpaces(Vector2 start, List<Vector2> aiTrail, List<Vector2> pTrail)
        {
            int c = 0;
            Vector2 pos = start;

            for (int i = 0; i < 10; i++)
            {
                if (IsOutOfBounds(pos) || CheckCollision(pos, aiTrail) || CheckCollision(pos, pTrail))
                {
                    break;
                }

                c++;
                pos = Move(pos, Dir.N, 10.0f);
            }

            return c;
        }

        static Vector2 Move(Vector2 pos, Dir dir, float v)
        {
            return dir switch
            {
                Dir.N => new(pos.X, pos.Y - v), // Move up
                Dir.S => new(pos.X, pos.Y + v), // Move down
                Dir.W => new(pos.X - v, pos.Y), // Move left
                Dir.E => new(pos.X + v, pos.Y), // Move right
                _ => pos
            };
        }

        static bool IsOutOfBounds(Vector2 pos)
        {
            return (pos.X < 0 || pos.X >= w || pos.Y < scoreBarH || pos.Y >= h - scoreBarH);
        }

        static bool CheckCollision(Vector2 pos, List<Vector2> trail)
        {
            for (int i = 0; i < trail.Count; i++) // Start from 1st element, not 0th
            {
                if (Vector2.Distance(pos, trail[i]) < 10.0f) // Check if player is too close to a trail segment
                {
                    return true;
                }
            }
            return false;
        }

        private static void RestartGame()
        {
            pPos = new(200, 300);
            aiPos = new(400, 300);
            pDir = aiDir = Dir.N;
            pTrail.Clear();
            aiTrail.Clear();
            gameRun = true;
        }

        static void SaveScores(string pName, int pScore, string aiName, int aiScore)
        {
            HiScoreDBHelper db_helper = new();
            db_helper.AddScore(pName, pScore, aiName, aiScore);
        }
    }
}

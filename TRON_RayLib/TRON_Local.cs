using Raylib_cs;
using Color = Raylib_cs.Color;
using System.Numerics;
using System.Collections.Generic;
using System.IO;

namespace TRON_RayLib
{
    internal class TRON_Local
    {
        static int w, h, scoreBarH;
        
        static Vector2 p1Pos, p2Pos;
        static List<Vector2> p1Trail, p2Trail;
        static int p1Score = 0, p2Score = 0;
        static float speed = 10.0f; // Movement speed is in terms of grid units

        static float moveDelay = 0.1f; // Delay in seconds between movement steps
        static float moveTimer = 0.0f; // Timer to track time between steps

        public enum Dir { N, S, W, E }
        static Dir p1Dir = Dir.E;
        static Dir p2Dir = Dir.W;

        static bool gameRunning = true;
        static bool gameOver = false; // To indicate when the game ends
        static string p1Name = "", p2Name = ""; // Variables to store player names
        static bool isGameReady = false; // Flag to track when both names are entered
        static bool isPlayer1EnteringName = true; // Player 1 enters their name first

        public static void StartGame()
        {
            w = h = 1000;
            scoreBarH = 60;
            Raylib.InitWindow(w, h, "PHANTASMATRON - LOCAL MULTI");
            Raylib.SetTargetFPS(60);

            p1Name = "";
            p2Name = ""; // Store player names
            isGameReady = false; // Game starts when both names are entered
            isPlayer1EnteringName = true; // Flag to track name input state
            gameOver = false; // Flag for game over
            gameRunning = true;

            // Game loop
            while (!Raylib.WindowShouldClose())
            {
                if (!isGameReady)
                {
                    // Player 1 Name Input
                    if (isPlayer1EnteringName)
                    {
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Color.Black);

                        Raylib.DrawText("Enter Player 1's Name:", w/10+50, h/5+50, 25, Color.White);
                        Raylib.DrawText(p1Name, w/10+100, h/5+100, 25, Color.SkyBlue);
                        HandleTextInput(ref p1Name, ref isPlayer1EnteringName); // Handle input for Player 1

                        Raylib.EndDrawing();
                    }
                    // Player 2 Name Input
                    else
                    {
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Color.Black);

                        Raylib.DrawText("Enter Player 2's Name:", w/10+50, h/5+50, 25, Color.White);
                        Raylib.DrawText(p2Name, w/10+100, h/5+100, 25, Color.Orange);
                        HandleTextInput(ref p2Name, ref isGameReady); // Handle input for Player 2

                        Raylib.EndDrawing();
                    }
                }
                else
                {
                    // If the game is over, wait for user input to restart
                    if (gameOver)
                    {
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Color.Black);

                        // Display game over message
                        Raylib.DrawText("GAME OVER!", w/10*3, h/5+200, 40, Color.Red);
                        Raylib.DrawText("Press R to restart or ESC to exit.", w/5*3, h/5+250, 20, Color.White);

                        Raylib.EndDrawing();

                        // Wait for input to restart or exit
                        if (Raylib.IsKeyPressed(KeyboardKey.R))
                        {
                            RestartGame(ref p1Name, ref p2Name); // Restart the game
                            gameOver = false; // Reset the game over state
                        }
                        else if (Raylib.IsKeyPressed(KeyboardKey.Escape))
                        {
                            SaveScores(p1Name, p1Score, p2Name, p2Score);
                            break; // Exit the game
                        }
                    }
                    else
                    {
                        // Continue running the game if not over
                        RunGame(ref p1Name, ref p2Name, ref gameOver);
                    }
                }
            }

            Raylib.CloseWindow();

            Raylib.CloseWindow();
            MainMenuForm obj = new();
            obj.Show();
        }

        // Method to handle text input
        static void HandleTextInput(ref string playerName, ref bool stateChange)
        {
            int key = Raylib.GetCharPressed();

            // Check for alphabetical character input
            while (key > 0)
            {
                if ((key >= 32) && (key <= 125) && playerName.Length < 12) // Limit name length to 12 chars
                {
                    playerName += (char)key; // Add the character to the name
                }
                key = Raylib.GetCharPressed(); // Get the next character
            }

            // Handle backspace
            if (Raylib.IsKeyPressed(KeyboardKey.Backspace) && playerName.Length > 0)
            {
                playerName = playerName[..^1]; // Remove last character
            }

            // Handle Enter key to confirm name
            if (Raylib.IsKeyPressed(KeyboardKey.Enter))
            {
                stateChange = !stateChange; // Move to the next player or start the game
            }
        }

        // The actual game logic after names have been entered
        static void RunGame(ref string p1Name, ref string p2Name, ref bool gameOver)
        {
            p1Pos = new(200, 300);
            p2Pos = new(400, 300);
            p1Trail = [];
            p2Trail = [];

            p1Dir = p2Dir = Dir.N;

            moveDelay = 0.01f;
            moveTimer = 0.0f;
            gameRunning = true;

            while (!Raylib.WindowShouldClose() && gameRunning)
            {
                // Update move timer
                moveTimer += Raylib.GetFrameTime();

                if (moveTimer >= moveDelay)
                {
                    // **Updated Player Controls to Prevent 180-degree Turns**

                    // Player 1 controls (W, A, S, D to move)
                    if (Raylib.IsKeyPressed(KeyboardKey.A) && p1Dir != Dir.E) p1Dir = Dir.W; // Go West (prevent 180 turn)
                    if (Raylib.IsKeyPressed(KeyboardKey.D) && p1Dir != Dir.W) p1Dir = Dir.E; // Go East (prevent 180 turn)
                    if (Raylib.IsKeyPressed(KeyboardKey.W) && p1Dir != Dir.S) p1Dir = Dir.N; // Go North (prevent 180 turn)
                    if (Raylib.IsKeyPressed(KeyboardKey.S) && p1Dir != Dir.N) p1Dir = Dir.S; // Go South (prevent 180 turn)

                    // Player 2 controls (Up, Down, Left, Right to move)
                    if (Raylib.IsKeyPressed(KeyboardKey.Left) && p2Dir != Dir.E) p2Dir = Dir.W; // Go West (prevent 180 turn)
                    if (Raylib.IsKeyPressed(KeyboardKey.Right) && p2Dir != Dir.W) p2Dir = Dir.E; // Go East (prevent 180 turn)
                    if (Raylib.IsKeyPressed(KeyboardKey.Up) && p2Dir != Dir.S) p2Dir = Dir.N; // Go North (prevent opposite turn)
                    if (Raylib.IsKeyPressed(KeyboardKey.Down) && p2Dir != Dir.N) p2Dir = Dir.S; // Go South (prevent opposite turn)

                    // Update positions based on directions
                    Vector2 newP1Pos = MovePlayer(p1Pos, p1Dir, speed);
                    Vector2 newP2Pos = MovePlayer(p2Pos, p2Dir, speed);

                    // Collision detection
                    if (CheckCollision(newP1Pos, p1Trail) || CheckCollision(newP1Pos, p2Trail) || IsOutOfBounds(newP1Pos))
                    {
                        gameOver = true;
                        break;
                    }

                    if (CheckCollision(newP2Pos, p2Trail) || CheckCollision(newP2Pos, p1Trail) || IsOutOfBounds(newP2Pos))
                    {
                        gameOver = true;
                        break;
                    }

                    p1Trail.Add(p1Pos);
                    p2Trail.Add(p2Pos);

                    p1Pos = newP1Pos;
                    p2Pos = newP2Pos;

                    moveTimer = 0.0f;
                }

                // Draw everything
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.DrawRectangle(0, 0, w, scoreBarH, Color.DarkGray);
                Raylib.DrawText($"{p1Name} (Player 1)", w/50, h/50, 20, Color.SkyBlue);
                Raylib.DrawText($"{p2Name} (Player 2)", w/10*8, h/50, 20, Color.Orange);

                Raylib.DrawText($"{p1Score}", w/50, h/50*2, 20, Color.SkyBlue);
                Raylib.DrawText($"{p2Score}", w/10*8, h/50*2, 20, Color.Orange);

                Raylib.DrawRectangleV(p1Pos, new Vector2(10, 10), Color.Blue);
                Raylib.DrawRectangleV(p2Pos, new Vector2(10, 10), Color.Red);

                foreach (Vector2 pos in p1Trail)
                {
                    Raylib.DrawRectangleV(pos, new(10, 10), Color.SkyBlue);
                }

                foreach (Vector2 pos in p2Trail)
                {
                    Raylib.DrawRectangleV(pos, new(10, 10), Color.Orange);
                }

                Raylib.EndDrawing();
            }
        }

        // Restart the game by resetting necessary variables
        static void RestartGame(ref string p1Name, ref string p2Name)
        {
            // Reset player positions, directions, and trails
            p1Pos = new(200, 300);
            p2Pos = new(400, 300);
            p1Dir = p2Dir = Dir.N;
            p1Trail.Clear();
            p2Trail.Clear();
            gameRunning = true;
        }

        // Helper functions (TurnLeft, TurnRight, MovePlayer, CheckCollision, IsOutOfBounds, SaveScores)
        static Vector2 MovePlayer(Vector2 position, Dir direction, float speed)
        {
            return direction switch
            {
                Dir.N => new Vector2(position.X, position.Y - speed), // Move up
                Dir.S => new Vector2(position.X, position.Y + speed), // Move down
                Dir.W => new Vector2(position.X - speed, position.Y), // Move left
                Dir.E => new Vector2(position.X + speed, position.Y), // Move right
                _ => position
            };
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

        static bool IsOutOfBounds(Vector2 pos)
        {
            return (pos.X < 0 || pos.X >= w || pos.Y < scoreBarH || pos.Y >= h - scoreBarH);
        }

        static void SaveScores(string p1Name, int p1Score, string p2Name, int p2Score)
        {
            HiScoreDBHelper db_helper = new();
            db_helper.AddScore(p1Name, p1Score, p2Name, p2Score);
        }
    }
}

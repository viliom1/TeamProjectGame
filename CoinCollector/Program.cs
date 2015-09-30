using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoinCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            int level = 1;
            int totalScore = 0;
            int coinsNumber = 5;
            int wallNumber = 15;

            while (level < 6)
            {
                int points = 0; //score tracking
                int wallHits = 0; // wallhits tracking
                int steps = 0; // steps tracking

                bool isCoin = false;
                string[,] matrix = new string[22, 22];
                int[] position = { 1, 1 }; // position of the "@" in the array
                matrix = BoardGen(matrix);
                string[,] board = WallGenerator(matrix, wallNumber, level);
                board = CoinGen(board, coinsNumber, level);
                int[] consolePos = { 3, 2 }; // position of the "@" on the console

                //if (level != 1)
                //{
                //    coinsNumber++;
                //    wallNumber += 3;
                //}

                BoardPrint(board, points, wallHits, steps, level, totalScore);
                ColorCoinsAndWalls(board);
                BoardCorrection(board);

                while (true)
                {
                    ConsoleKeyInfo movement = Console.ReadKey(); // reading the key
                    // now checking what key was read
                    if (movement.Key == ConsoleKey.RightArrow)
                    {
                        bool wall = IsWallRight(board, position);
                        StepsUpdate(steps);
                        steps++;
                        if (wall == true)
                        {
                            WallHitsUpdate(wallHits);
                            wallHits++;

                        }
                        else
                        {
                            isCoin = IsCoinRight(board, position);
                            if (isCoin == true)
                            {
                                //Console.Beep();
                                CoinCollected(points);
                                points += 100;
                            }

                            MoveRight(position, consolePos);

                            isCoin = false;
                        }
                    }
                    else if (movement.Key == ConsoleKey.LeftArrow)
                    {

                        bool wall = IsWallLeft(board, position);
                        StepsUpdate(steps);
                        if (wall == true)
                        {
                            WallHitsUpdate(wallHits);
                            wallHits++;
                        }
                        else
                        {
                            isCoin = IsCoinLeft(board, position);
                            if (isCoin == true)
                            {
                                //Console.Beep();
                                CoinCollected(points);
                                points += 100;
                            }


                            MoveLeft(position, consolePos);

                        }
                    }
                    else if (movement.Key == ConsoleKey.DownArrow)
                    {
                        bool wall = IsWallDown(board, position);
                        StepsUpdate(steps);
                        steps++;
                        if (wall == true)
                        {
                            WallHitsUpdate(wallHits);
                            wallHits++;
                        }

                        else
                        {
                            isCoin = IsCoinDown(board, position);
                            if (isCoin == true)
                            {
                                //Console.Beep();
                                CoinCollected(points);
                                points += 100; ;
                            }


                            MoveDown(position, consolePos);

                        }
                    }
                    else if (movement.Key == ConsoleKey.UpArrow)
                    {
                        bool wall = IsWallUp(board, position);
                        StepsUpdate(steps);
                        steps++;
                        if (wall == true)
                        {
                            WallHitsUpdate(wallHits);
                            wallHits++;
                        }
                        else
                        {
                            isCoin = IsCoinUp(board, position);
                            if (isCoin == true)
                            {
                                //Console.Beep();
                                CoinCollected(points);
                                points += 100;
                            }


                            MoveUp(position, consolePos);

                        }
                    }
                    if (points == coinsNumber * 100)
                    {
                        break;
                    }
                }


                Console.WriteLine();
                int finalScore = points - wallHits * 10 - steps; // calculating the final score
                totalScore += finalScore;

                if (finalScore > 0)
                {
                    if (level != 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Proceeding to the next level with score of {0} points", finalScore);

                    }
                    else
                    {
                        totalScore += finalScore;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("CONGRATULATIONS!!!YOU WON THE GAME WITH TOTAL SCORE OF {0} POINTS", totalScore);
                        break;
                    }

                    level++;
                    coinsNumber++;
                    wallNumber += 3;


                    //Console.WriteLine("GEEEEGEEEEE!!!!!! Your score is: {0}", finalScore);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Your score is: {0} points. Start from the begining.", finalScore);
                }
                //else
                //{
                //    Console.WriteLine("GAME OVER !!! Your score is 0");
                //}

                Console.ReadKey();
                Console.Clear();
            }

        }

        static void BoardPrint(string[,] matrix, int points, int wallHits, int steps, int level, int totalScore)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Points:{0}", points);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("   Hits:{0}", wallHits);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("   Steps:{0}", steps);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("   Level:{0}", level);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("   Total Score:{0}", totalScore);
            Console.ResetColor();
            Console.WriteLine();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0,2}", matrix[i, j]);
                }
                Console.WriteLine();
            }
        }
        static string[,] CoinGen(string[,] matrix, int coinsNumber, int level)
        {
            //if (level != 1)
            //{
            //    coinsNumber++;
            //}

            Random generator = new Random();

            for (int i = 0; i < coinsNumber; i++)
            {

                int rndRow = generator.Next(1, 18);
                int rndCol = generator.Next(1, 18);
                if (matrix[rndRow, rndCol] == "$" || matrix[rndRow, rndCol] == "@")
                {
                    matrix[rndCol, rndRow] = "$";
                }
                else
                {
                    matrix[rndRow, rndCol] = "$";
                }
            }
            return matrix;
        }
        static string[,] BoardGen(string[,] matrix)
        {
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 22; j++)
                {
                    if (i == 1 && j == 1)
                    {
                        matrix[i, j] = "@";
                    }
                    else
                    {
                        if (i == 0 || i == 21)
                        {
                            matrix[i, j] = "_";
                        }
                        else if (j == 0 || j == 21)
                        {
                            matrix[i, j] = "|";
                        }
                        else
                        {
                            matrix[i, j] = ".";
                        }

                    }
                }
            }
            return matrix;
        }
        static void MoveRight(int[] position, int[] consolePos)
        {
            Console.SetCursorPosition(consolePos[0], consolePos[1]); // setting the cursor to the current position of the "@"
            Console.Write(". ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("@");
            Console.ResetColor();
            Console.SetCursorPosition(0, 22); // moving the cursor out of the board
            position[1]++; // updating the position
            consolePos[0] += 2;// and the console position

        }   // Move**** are pringting on the console with the movement you want
        static void MoveLeft(int[] position, int[] consolePos)
        {
            Console.SetCursorPosition(consolePos[0] - 2, consolePos[1]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("@");
            Console.ResetColor();
            Console.Write(" .");
            Console.SetCursorPosition(0, 22);
            position[1]--;
            consolePos[0] -= 2;
        }
        static void MoveDown(int[] position, int[] consolePos)
        {
            Console.SetCursorPosition(consolePos[0], consolePos[1]);
            Console.Write(".");
            Console.SetCursorPosition(consolePos[0], consolePos[1] + 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("@");
            Console.ResetColor();
            Console.SetCursorPosition(0, 22);
            position[0]++;
            consolePos[1]++; ;
        }
        static void MoveUp(int[] position, int[] consolePos)
        {
            Console.SetCursorPosition(consolePos[0], consolePos[1]);
            Console.Write(".");
            Console.SetCursorPosition(consolePos[0], consolePos[1] - 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("@");
            Console.ResetColor();
            Console.SetCursorPosition(0, 22);
            position[0]--;
            consolePos[1]--;
        }
        static bool IsCoinRight(string[,] matrix, int[] positon)
        {
            bool coin = false;
            if (matrix[positon[0], positon[1] + 1] == "$")
            {
                matrix[positon[0], positon[1] + 1] = ".";
                coin = true;
            }

            return coin;
        }  //IsCoin*** checks if the next movement lands on a coin
        static bool IsCoinLeft(string[,] matrix, int[] positon)
        {
            bool coin = false;
            if (matrix[positon[0], positon[1] - 1] == "$")
            {
                matrix[positon[0], positon[1] - 1] = ".";
                coin = true;
            }
            return coin;
        }
        static bool IsCoinDown(string[,] matrix, int[] positon)
        {
            bool coin = false;
            if (matrix[positon[0] + 1, positon[1]] == "$")
            {
                matrix[positon[0] + 1, positon[1]] = ".";
                coin = true;
            }
            return coin;
        }
        static bool IsCoinUp(string[,] matrix, int[] positon)
        {
            bool coin = false;
            if (matrix[positon[0] - 1, positon[1]] == "$")
            {
                matrix[positon[0] - 1, positon[1]] = ".";
                coin = true;
            }
            return coin;
        }
        static bool IsWallRight(string[,] matrix, int[] position)
        {
            bool wall = false;
            if (matrix.GetLength(0) == position[1] + 1 || matrix[position[0], position[1] + 1] == "|")
            {
                wall = true;
            }
            return wall;
        } //IsWall*** checks if the next movement lands on a wall
        static bool IsWallLeft(string[,] matrix, int[] position)
        {
            bool wall = false;
            if (0 > position[1] - 1 || matrix[position[0], position[1] - 1] == "|")
            {
                wall = true;
            }
            return wall;
        }
        static bool IsWallDown(string[,] matrix, int[] position)
        {
            bool wall = false;
            if (matrix[position[0] + 1, position[1]] == "_" || matrix[position[0] + 1, position[1]] == "|")
            {
                wall = true;
            }


            return wall;
        }
        static bool IsWallUp(string[,] matrix, int[] position)
        {
            bool wall = false;
            if (matrix[position[0] - 1, position[1]] == "_" || matrix[position[0] - 1, position[1]] == "|")
            {
                wall = true;
            }
            return wall;
        }
        static string[,] WallGenerator(string[,] matrix, int wallNumber, int level)
        {
            if (level != 1)
            {
                wallNumber += 3;
            }

            Random generator = new Random();
            for (int i = 0; i < wallNumber; i++)
            {

                int rndRow = generator.Next(1, 17);
                int rndCol = generator.Next(1, 17);
                if (matrix[rndRow, rndCol] == "$" || matrix[rndRow, rndCol] == "|")
                {
                    matrix[rndRow + 1, rndCol + 1] = "|";
                }
                else
                {
                    matrix[rndRow, rndCol] = "|";
                }
            }
            return matrix;

        }    // generates walls on the board in random positions
        static void CoinCollected(int points)
        {
            points += 100;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(7, 0);
            Console.Write(points);
            Console.ResetColor();
        }  // updates the score
        static void StepsUpdate(int steps)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            steps++;
            Console.SetCursorPosition(26, 0);
            Console.Write(steps);
            Console.ResetColor();
        } // updates the steps
        static void WallHitsUpdate(int wallHits)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            wallHits++;
            Console.SetCursorPosition(16, 0);
            Console.Write(wallHits);
            Console.SetCursorPosition(0, 22);
            Console.ResetColor();
        } //updates the hits
        static void ColorCoinsAndWalls(string[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == "$")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.SetCursorPosition(j * 2 + 1, i + 1);
                        Console.Write("$");
                        Console.ResetColor();
                        Console.SetCursorPosition(0, 22);
                    }
                    else if (board[i, j] == "|")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(j * 2 + 1, i + 1);
                        Console.Write("|");
                        Console.ResetColor();
                        Console.SetCursorPosition(0, 22);
                    }
                    else if (board[i, j] == "_")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(j * 2 + 1, i + 1);
                        Console.Write("_");
                        Console.ResetColor();
                        Console.SetCursorPosition(0, 22);
                    }

                    else if (board[i, j] == "@")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.SetCursorPosition(j * 2 + 1, i + 1);
                        Console.Write("@");
                        Console.ResetColor();
                        Console.SetCursorPosition(0, 22);
                    }

                }
            }
        }
        static void BoardCorrection(string[,] board)
        {
            for (int i = 1; i < board.GetLength(0) - 1; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(0, i + 1);
                Console.Write(" | ");

            }
            for (int i = 1; i < board.GetLength(1) * 2; i++)
            {
                //Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(i, 1);
                Console.Write("_");
                Console.SetCursorPosition(i, 22);
                Console.Write("_");
            }
        }
    }
}

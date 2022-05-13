using System;
using System.IO;

namespace N_Puzzle
{
    class Program
    {
        static string[] SampleTests = new string[]
        {
            "Testcases/Sample/Sample Test/Solvable Puzzles/8 Puzzle (1).txt",
            "Testcases/Sample/Sample Test/Solvable Puzzles/8 Puzzle (2).txt",
            "Testcases/Sample/Sample Test/Solvable Puzzles/8 Puzzle (3).txt",
            "Testcases/Sample/Sample Test/Solvable Puzzles/15 Puzzle - 1.txt",
            "Testcases/Sample/Sample Test/Solvable Puzzles/24 Puzzle 1.txt",
            "Testcases/Sample/Sample Test/Solvable Puzzles/24 Puzzle 2.txt",
            "Testcases/Sample/Sample Test/Unsolvable Puzzles/8 Puzzle - Case 1.txt",
            "Testcases/Sample/Sample Test/Unsolvable Puzzles/8 Puzzle(2) - Case 1.txt",
            "Testcases/Sample/Sample Test/Unsolvable Puzzles/8 Puzzle(3) - Case 1.txt",
            "Testcases/Sample/Sample Test/Unsolvable Puzzles/15 Puzzle - Case 2.txt",
            "Testcases/Sample/Sample Test/Unsolvable Puzzles/15 Puzzle - Case 3.txt"
        };

        static string[] CompleteTests = new string[]
        {
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan & Hamming/50 Puzzle.txt",
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan & Hamming/99 Puzzle - 1.txt",
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan & Hamming/99 Puzzle - 2.txt",
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan & Hamming/9999 Puzzle.txt",
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan Only/15 Puzzle 1.txt",
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan Only/15 Puzzle 3.txt",
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan Only/15 Puzzle 4.txt",
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan Only/15 Puzzle 5.txt",
            "Testcases/Complete/Complete Test/Unsolvable puzzles/15 Puzzle 1 - Unsolvable.txt",
            "Testcases/Complete/Complete Test/Unsolvable puzzles/99 Puzzle - Unsolvable Case 1.txt",
            "Testcases/Complete/Complete Test/Unsolvable puzzles/99 Puzzle - Unsolvable Case 2.txt",
            "Testcases/Complete/Complete Test/Unsolvable puzzles/9999 Puzzle.txt",
            "Testcases/Complete/Complete Test/V. Large test case/TEST.txt",
        };

        static int[,] Puzzle;
        static int[] Puzzle_1D;
        static int Empty_i_Pos, Empty_j_Pos;

        static void Main(string[] args)
        {
            // Reading the File
            FileStream file = new FileStream(SampleTests[1], FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            // Getting the Problem Size
            int PuzzleSize = int.Parse(sr.ReadLine());

            // Putting the Puzzle Values into a Matrix
            FormPuzzle(sr, PuzzleSize);
            DisplayPuzzle(PuzzleSize);

            if (CheckIsSolvable(Puzzle, PuzzleSize))
            {
                Console.WriteLine("Solvable.");
                int ManDist = ManhattanPriorityDistance(Puzzle, PuzzleSize);
                Console.WriteLine(ManDist);
            }
            else
                Console.WriteLine("UnSolvable.");
        }

        static void DisplaySampleCases()
        {
            for (int i = 0; i < SampleTests.Length; i++)
            {
                FileStream file = new FileStream(SampleTests[i], FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file);
                Console.WriteLine("---------------------- Sample Case " + (i + 1) + "----------------------");
                Console.WriteLine(sr.ReadToEnd());
            }
        }
        static void DisplayCompleteCases()
        {
            for (int i = 0; i < CompleteTests.Length; i++)
            {
                FileStream file = new FileStream(CompleteTests[i], FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file);
                Console.WriteLine("---------------------- Complete Case " + (i + 1) + "----------------------");
                Console.WriteLine(sr.ReadToEnd());
            }
        }

        static void FormPuzzle(StreamReader sr, int S)
        {
            Puzzle = new int[S,S];
            string line = sr.ReadLine();
            string[] PuzzleValues = line.Split(' ');
            for (int i = 0; i < S; i++)
            {
                line = sr.ReadLine();
                if(line != null)
                    PuzzleValues = line.Split(' ');
                for (int j = 0; j < S; j++)
                {
                    Puzzle[i, j] = int.Parse(PuzzleValues[j]);
                    if (int.Parse(PuzzleValues[j]) == 0)
                    {
                        Empty_i_Pos = i; Empty_j_Pos = j;
                    }
                }
            }
            ConvertTo1DPuzzle(S);
        }

        static void ConvertTo1DPuzzle(int S)
        {
            Puzzle_1D = new int[S * S];
            for (int i = 0; i < Puzzle.Length; i++)
                Puzzle_1D[i] = Puzzle[i / S, i % S];
        }

        static void DisplayPuzzle(int S)
        {
            for (int i = 0; i < S; i++)
            {
                for (int j = 0; j < S; j++)
                    Console.Write(Puzzle[i, j] + " ");
                Console.WriteLine();
            }
            Console.WriteLine("The Empty Cell is at: (" + Empty_i_Pos + " ," + Empty_j_Pos + ")");
            Console.WriteLine("------------------------- 1D Array Values -------------------------");
            for (int i = 0; i < Puzzle.Length; i++)
                Console.Write(Puzzle_1D[i] + " ");
            Console.WriteLine();
        }

        static bool CheckIsSolvable(int[,] Puzzle, int S)
        {
            int InversionCount = 0;
            int EmptyUpIndex = Empty_i_Pos + 1;
            int EmptyDownIndex = Empty_j_Pos + 1;
            for (int i = 0; i < Puzzle.Length-1; i++)
            {
                for (int j = i; j < Puzzle.Length; j++)
                {
                    if (Puzzle_1D[i] > Puzzle_1D[j])
                        if(Puzzle_1D[j] != 0)
                            InversionCount++;
                }
            }
            EmptyDownIndex = S + 1 - EmptyUpIndex;
            if (S % 2 != 0 && InversionCount % 2 == 0)
                return true;
            else if (S % 2 == 0 && InversionCount % 2 == 0 && EmptyDownIndex % 2 != 0)
                return true;
            else if (S % 2 == 0 && InversionCount % 2 != 0 && EmptyDownIndex % 2 == 0)
                return true;
            return false;
        }

        static int ManhattanPriorityDistance (int[,] Puzzle, int S)
        {
            int ManhattanDistance = 0;
            for (int i = 0; i < Puzzle.Length; i++)
            {
                if(Puzzle_1D[i] != i+1 && Puzzle_1D[i] != 0)
                {
                    int CorrectRowPosition = (Puzzle_1D[i] - 1) / S;
                    int CorrectColumnPosition = (Puzzle_1D[i] - 1) % S;
                    ManhattanDistance += Math.Abs(CorrectRowPosition - i/S) + Math.Abs(CorrectColumnPosition - i%S);
                }
            }
            return ManhattanDistance;
        }
    }
}

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
        static PriorityQueue NodesQueue;

        static void Main(string[] args)
        {
            // Reading the File
            FileStream file = new FileStream(SampleTests[0], FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            // Getting the Problem Size
            int PuzzleSize = int.Parse(sr.ReadLine());

            // Putting the Puzzle Values into a Matrix
            FormPuzzle(sr, PuzzleSize);

            // Check if the Puzzle is Solvable
            if (CheckIsSolvable(Puzzle, PuzzleSize))
            {
                Console.WriteLine("Solvable.\n");
                DisplayPuzzle(Puzzle, Puzzle_1D, PuzzleSize, Empty_i_Pos, Empty_j_Pos);
                int ManDist = ManhattanPriorityDistance(Puzzle, Puzzle_1D, PuzzleSize);
                int HamDist = HammingPriorityDistance(Puzzle, Puzzle_1D, PuzzleSize);
                Console.WriteLine("Manhatan Distance: " + ManDist + "\nHamming Distance: " + HamDist);

                PuzzleNode InitialPuzzle = new PuzzleNode(Puzzle, PuzzleSize, Empty_i_Pos, Empty_j_Pos);
                A_Star_Algorithm(InitialPuzzle, 'M'); // Manhattan Distance

                PuzzleNode pn = new PuzzleNode(Puzzle, PuzzleSize, Empty_i_Pos, Empty_j_Pos);
                PuzzleNode pnRight = new PuzzleNode(pn.RightDirection(pn));
                DisplayPuzzle(pnRight.Puzzle, pnRight.Puzzle_1D, pnRight.S, pnRight.Empty_i_Pos, pnRight.Empty_j_Pos);
                int RightManDist = ManhattanPriorityDistance(pnRight.Puzzle, pnRight.Puzzle_1D, pnRight.S);
                int RightHamDist = HammingPriorityDistance(pnRight.Puzzle, pnRight.Puzzle_1D, pnRight.S);
                Console.WriteLine("Manhatan Distance: " + RightManDist + "\nHamming Distance: " + RightHamDist);


                PuzzleNode pn2 = new PuzzleNode(Puzzle, PuzzleSize, Empty_i_Pos, Empty_j_Pos);
                PuzzleNode pnDown = new PuzzleNode(pn.DownDirection(pn));
                DisplayPuzzle(pnDown.Puzzle, pnDown.Puzzle_1D, pnDown.S, pnDown.Empty_i_Pos, pnDown.Empty_j_Pos);
                int DownManDist = ManhattanPriorityDistance(pnDown.Puzzle, pnDown.Puzzle_1D, pnDown.S);
                int DownHamDist = HammingPriorityDistance(pnDown.Puzzle, pnDown.Puzzle_1D, pnDown.S);
                Console.WriteLine("Manhatan Distance: " + DownManDist + "\nHamming Distance: " + DownHamDist);
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
            ConvertTo1DPuzzle(Puzzle, S);
        }
        static void ConvertTo1DPuzzle(int[,] Puzzle, int S)
        {
            Puzzle_1D = new int[S * S];
            for (int i = 0; i < Puzzle.Length; i++)
                Puzzle_1D[i] = Puzzle[i / S, i % S];
        }
        static void DisplayPuzzle(int[,] Puzzle, int[]Puzzle_1D, int S, int ei, int ej)
        {
            Console.WriteLine("\n========================= Puzzle Details =========================");
            Console.WriteLine("==================================================================");
            for (int i = 0; i < S; i++)
            {
                for (int j = 0; j < S; j++)
                    Console.Write(Puzzle[i, j] + " ");
                Console.WriteLine();
            }
            Console.WriteLine("The Empty Cell is at: (" + ei + " ," + ej + ")");
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
        public static int ManhattanPriorityDistance (int[,] Puzzle, int[] Puzzle_1D, int S)
        {
            int ManhattanDistance = 0;
            for (int i = 0; i < Puzzle.Length; i++)
            {
                if(Puzzle_1D[i] != i+1 && Puzzle_1D[i] != 0)
                {
                    int RowPosition = (Puzzle_1D[i] - 1) / S;
                    int ColumnPosition = (Puzzle_1D[i] - 1) % S;
                    int CorrectRowPosition = Math.Abs(RowPosition - i / S);
                    int CorrectColumnPosition = Math.Abs(ColumnPosition - i % S);
                    ManhattanDistance += CorrectRowPosition + CorrectColumnPosition;
                }
            }
            return ManhattanDistance;
        }
        public static int HammingPriorityDistance (int[,] Puzzle, int[] Puzzle_1D, int S)
        {
            int HammingDistance = 0;
            for (int i = 0; i < Puzzle.Length; i++)
            {
                if (Puzzle_1D[i] != i + 1 && Puzzle_1D[i] != 0)
                    HammingDistance++;
            }

            return HammingDistance;
        }
        static PuzzleNode A_Star_Algorithm(PuzzleNode InitialPuzzle, char Dist_Func)
        {
            NodesQueue = new PriorityQueue();
            PuzzleNode TreeRoot = InitialPuzzle;
            NodesQueue.Enqueue(TreeRoot, Dist_Func);

            return TreeRoot;
        }
    }
}

using System;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace Team_122_N_Puzzle
{
    class Program
    {
        static string[] SampleTests = new string[]
        {
            "Testcases/Sample/Sample Test/Solvable Puzzles/8 Puzzle (1).txt",           // 8
            "Testcases/Sample/Sample Test/Solvable Puzzles/8 Puzzle (2).txt",           // 20
            "Testcases/Sample/Sample Test/Solvable Puzzles/8 Puzzle (3).txt",           // 14
            "Testcases/Sample/Sample Test/Solvable Puzzles/15 Puzzle - 1.txt",          // 5
            "Testcases/Sample/Sample Test/Solvable Puzzles/24 Puzzle 1.txt",            // 11
            "Testcases/Sample/Sample Test/Solvable Puzzles/24 Puzzle 2.txt",            // 24
            "Testcases/Sample/Sample Test/Unsolvable Puzzles/8 Puzzle - Case 1.txt",
            "Testcases/Sample/Sample Test/Unsolvable Puzzles/8 Puzzle(2) - Case 1.txt",
            "Testcases/Sample/Sample Test/Unsolvable Puzzles/8 Puzzle(3) - Case 1.txt",
            "Testcases/Sample/Sample Test/Unsolvable Puzzles/15 Puzzle - Case 2.txt",
            "Testcases/Sample/Sample Test/Unsolvable Puzzles/15 Puzzle - Case 3.txt"
        };
        static string[] CompleteTests = new string[]
        {
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan & Hamming/50 Puzzle.txt",      // 18
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan & Hamming/99 Puzzle - 1.txt",  // 18
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan & Hamming/99 Puzzle - 2.txt",  // 38
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan & Hamming/9999 Puzzle.txt",    // 4
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan Only/15 Puzzle 1.txt",         // 46
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan Only/15 Puzzle 3.txt",         // 38
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan Only/15 Puzzle 4.txt",         // 44
            "Testcases/Complete/Complete Test/Solvable puzzles/Manhattan Only/15 Puzzle 5.txt",         // 45
            "Testcases/Complete/Complete Test/Unsolvable puzzles/15 Puzzle 1 - Unsolvable.txt",
            "Testcases/Complete/Complete Test/Unsolvable puzzles/99 Puzzle - Unsolvable Case 1.txt",
            "Testcases/Complete/Complete Test/Unsolvable puzzles/99 Puzzle - Unsolvable Case 2.txt",
            "Testcases/Complete/Complete Test/Unsolvable puzzles/9999 Puzzle.txt",
            "Testcases/Complete/Complete Test/V. Large test case/TEST.txt",                             // 56
        };
        static int[,] Puzzle;
        static int PuzzleSize;
        static int[] Puzzle_1D;
        static int Empty_i_Pos, Empty_j_Pos;
        static PriorityQueue Queue = new PriorityQueue();
        static void Main(string[] args)
        {
            UserDisplay();

            Console.Write("\n[1] Manhattan \n[2] Hamming \nEnter Your Option: ");
            int DistChoice = int.Parse(Console.ReadLine());
            bool IsManhattan = true;

            if (DistChoice == 1) { }
            else if (DistChoice == 2) { IsManhattan = false; }
            else { throw new Exception("Invalid Choice"); }

            Console.WriteLine("Processing ... \n===============================================================\n");

            /*Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            System.Threading.Thread.Sleep(500);*/

            int DistanceFunction = 0;
            if (CheckIsSolvable(Puzzle_1D, PuzzleSize))
            {
                Console.WriteLine("******************** Solvable ********************");
                if (IsManhattan)
                    DistanceFunction = ManhattanPriorityDistance(Puzzle, PuzzleSize);
                else
                    DistanceFunction = HammingPriorityDistance(Puzzle, PuzzleSize);

                PuzzleNode InitialNode = new PuzzleNode(Puzzle, PuzzleSize, Empty_i_Pos, Empty_j_Pos, DistanceFunction, 0, null);
                PuzzleNode Goal = A_Star_Algorithm(InitialNode, IsManhattan);
                Console.Write("*************** Number of Movements = {0} **************\n", Goal.DepthLevel);
                Display(Goal, IsManhattan);
            }
            else
                Console.WriteLine("Not Solvable");

            /*stopwatch.Stop();
            Console.WriteLine("Time : {0}", stopwatch.Elapsed);
            Console.Read();*/
        }
        public static void UserDisplay()
        {
            Console.Write("[1] Sample Tests \n[2] Complete Tests \nEnter Your Option: ");
            int TestChoice = int.Parse(Console.ReadLine());

            FileStream file = new FileStream(CompleteTests[0], FileMode.Open, FileAccess.Read);

            if (TestChoice == 1)
            {
                for (int i = 0; i < SampleTests.Length; i++)
                    Console.Write("\n[{0}] {1}", i + 1, SampleTests[i]);
                Console.Write("\nEnter You Option: ");
                int Test = int.Parse(Console.ReadLine());
                if (Test > SampleTests.Length) { throw new Exception("Invalid Choice"); }
                file = new FileStream(SampleTests[Test - 1], FileMode.Open, FileAccess.Read);
            }
            else if (TestChoice == 2)
            {
                for (int i = 0; i < CompleteTests.Length; i++)
                    Console.Write("\n[{0}] {1}", i + 1, CompleteTests[i]);
                Console.Write("\nEnter You Option: ");
                int Test = int.Parse(Console.ReadLine());
                if (Test > CompleteTests.Length) { throw new Exception("Invalid Choice"); }
                file = new FileStream(CompleteTests[Test - 1], FileMode.Open, FileAccess.Read);
            }
            else { throw new Exception("Invalid Choice"); }

            StreamReader sr = new StreamReader(file);

            PuzzleSize = int.Parse(sr.ReadLine());
            FormPuzzle(sr, PuzzleSize);
            file.Close();
        }
        public static void FormPuzzle(StreamReader sr, int S)
        {
            Puzzle = new int[S, S];
            Puzzle_1D = new int[S * S];
            string Empty = sr.ReadLine();
            for (int i = 0; i < S; i++)
            {
                String[] PuzzleValues;
                if (Empty == "")
                    PuzzleValues = sr.ReadLine().Split(' ');
                else
                {
                    PuzzleValues = Empty.Split(' ');
                    Empty = "";
                }

                for (int j = 0; j < S; j++)
                {
                    Puzzle[i, j] = Puzzle_1D[i * S + j] = int.Parse(PuzzleValues[j]);
                    if (Puzzle[i, j] == 0) { Empty_i_Pos = i; Empty_j_Pos = j; }
                }
            }
            sr.Close();
        }
        public static bool CheckIsSolvable(int[] Puzzle_1D, int S)
        {
            int InversionCount = 0;
            int EmptyUpIndex = Empty_i_Pos + 1;
            int EmptyDownIndex = Empty_j_Pos + 1;
            for (int i = 0; i < Puzzle.Length - 1; i++)
            {
                for (int j = i; j < Puzzle.Length; j++)
                {
                    if (Puzzle_1D[i] > Puzzle_1D[j])
                        if (Puzzle_1D[j] != 0)
                            InversionCount++;
                }
            }
            EmptyDownIndex = S + 1 - EmptyUpIndex;
            if (S % 2 != 0 && InversionCount % 2 == 0)
                return true;
            else if (S % 2 == 0)
            {
                if (InversionCount % 2 == 0 && EmptyDownIndex % 2 != 0)
                    return true;
                else if (InversionCount % 2 != 0 && EmptyDownIndex % 2 == 0)
                    return true;
            }
            return false;
        }


        public static int ManhattanPriorityDistance(int[,] Puzzle, int S)
        {
            int ManhattanDistance = 0;
            for (int i = 0; i < Puzzle.Length; i++)
            {
                if (Puzzle_1D[i] != i + 1 && Puzzle_1D[i] != 0)
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
        public static int HammingPriorityDistance(int[,] Puzzle, int S)
        {
            int HammingDistance = 0;
            for (int i = 0; i < Puzzle.Length; i++)
            {
                if (Puzzle_1D[i] != i + 1 && Puzzle_1D[i] != 0)
                    HammingDistance++;
            }
            return HammingDistance;
        }

        public static PuzzleNode A_Star_Algorithm(PuzzleNode pn, bool IsManhattan)
        {
            PuzzleNode TreeRoot = pn;
            Queue.Enqueue(TreeRoot);
            while (true)
            {
                PuzzleNode NextMove = Queue.Dequeue();
                if (NextMove.Dist_value == 0)
                    return NextMove;

                NextMove.NextMove(IsManhattan);
                for (int i = 0; i < NextMove.Children.Count(); i++)
                {
                    PuzzleNode Child = NextMove.Children[i];
                    Child.TotalCost = Child.DepthLevel + Child.Dist_value;
                    Queue.Enqueue(Child);
                }
            }
        }
        public static void Display(PuzzleNode pn, bool IsManhattan)
        {
            if (pn == null)
                return;

            Display(pn.Parent, IsManhattan);
            for (int i = 0; i < pn.S; i++)
            {
                for (int j = 0; j < pn.S; j++)
                    Console.Write(pn.Puzzle[i, j] + " ");
                Console.WriteLine();
            }
            Console.WriteLine("The Empty Cell is at ({0},{1})", pn.Empty_i_Pos, pn.Empty_j_Pos);
            if (IsManhattan) { Console.WriteLine("Manhattan Distance is: {0} \n", pn.Dist_value); }
            else { Console.WriteLine("Hamming Distance is: {0} \n", pn.Dist_value); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Team_122_N_Puzzle
{
    class PuzzleNode
    {
        public int[,] Puzzle;
        public int[] Puzzle_1D;

        public int S;
        public int DepthLevel;
        public int Dist_value; //Hamming
        public int Empty_i_Pos;
        public int Empty_j_Pos;
        public int Moves = 0;
        public int Move_cost;
        public string Move_dir;
        public string MainKey;
        public int TotalCost;

        public PuzzleNode Parent;
        public List<PuzzleNode> Children;

        public bool CanMoveUp = false;
        public bool CanMoveRight = false;
        public bool CanMoveDown = false;
        public bool CanMoveLeft = false;

        //Main Constructor
        public PuzzleNode(int[,] Puzzle, int S, int Empty_i_Pos, int Empty_j_Pos, int Dist_value, int DepthLevel, PuzzleNode P)
        {
            this.S = S;
            this.Puzzle = Puzzle;

            this.DepthLevel = DepthLevel;
            this.Empty_i_Pos = Empty_i_Pos;
            this.Empty_j_Pos = Empty_j_Pos;
            this.Move_dir = "Initial";
            this.Dist_value = Dist_value;
            this.Parent = P;
            Children = new List<PuzzleNode>();
        }
        // Secondry Copy Constructor
        public PuzzleNode(PuzzleNode pn)
        {
            this.S = pn.S;
            this.Puzzle = new int[S, S];
            this.Puzzle_1D = new int[S * S];

            for (int i = 0; i < this.S * this.S; i++)
            {
                this.Puzzle[i / this.S, i % this.S] = pn.Puzzle[i / this.S, i % this.S];
                this.Puzzle_1D[i] = pn.Puzzle[i / this.S, i % this.S];
                this.MainKey = this.MainKey + Puzzle[i / this.S, i % this.S];
            }

            this.DepthLevel = pn.DepthLevel + 1;
            this.Empty_i_Pos = pn.Empty_i_Pos;
            this.Empty_j_Pos = pn.Empty_j_Pos;
            this.Move_dir = "To Be Determined";
            this.Move_cost = pn.Move_cost;
            this.MainKey = pn.MainKey;
            this.Parent = pn.Parent;
            this.Dist_value = pn.Dist_value;
        }

        public int ManCheckOptimal(PuzzleNode pn, int N, int LastEmpty_i_Pos, int LastEmpty_j_Pos)
        {
            int Manhatten = pn.Dist_value;
            int ParentCorrectPositon = LastEmpty_i_Pos * pn.S + LastEmpty_j_Pos;
            if (ParentCorrectPositon != N)
                Manhatten += ManCal(pn, N, pn.Empty_i_Pos, pn.Empty_j_Pos) - ManCal(pn, N, LastEmpty_i_Pos, LastEmpty_j_Pos);
            else
                Manhatten++;
            return Manhatten;
        }
        public int ManCal(PuzzleNode pn, int N, int LastEmpty_i_Pos, int LastEmpty_j_Pos)
        {
            int i_Pos = N / pn.S;
            int j_Pos = N % pn.S;
            int CorrectRowPosition = Math.Abs(i_Pos - LastEmpty_i_Pos);
            int CorrectColumnPosition = Math.Abs(j_Pos - LastEmpty_j_Pos);
            return CorrectRowPosition + CorrectColumnPosition;
        }
        public int HamCheckOptimal(PuzzleNode pn, int N, int LastEmpty_i_Pos, int LastEmpty_j_Pos)
        {
            int Ham = pn.Dist_value;
            int ChildCorrectPosition = pn.Empty_i_Pos * pn.S + pn.Empty_j_Pos;
            int ParentCorrectPositon = LastEmpty_i_Pos * pn.S + LastEmpty_j_Pos;
            if (ParentCorrectPositon == N - 1)
                Ham++;
            else if (ChildCorrectPosition == N - 1)
                Ham--;
            return Ham;
        }

        public void FeasibleMoves()
        {
            if (this.Empty_i_Pos > 0)
                this.CanMoveUp = true;
            if (this.Empty_i_Pos + 1 < S)
                this.CanMoveDown = true;
            if (this.Empty_j_Pos > 0)
                this.CanMoveLeft = true;
            if (this.Empty_j_Pos + 1 < S)
                this.CanMoveRight = true;
        }
        public void UpDirection(bool IsManhattan)
        {
            int[,] UpBranch = new int[S, S];
            for (int i = 0; i < Puzzle.Length; i++)
                UpBranch[i / S, i % S] = Puzzle[i / S, i % S];

            PuzzleNode CurrNode = this;
            int IndexAbove_0 = UpBranch[Empty_i_Pos - 1, Empty_j_Pos];

            int DistanceFunction = 0;
            if (IsManhattan)
                DistanceFunction = ManCheckOptimal(CurrNode, IndexAbove_0 - 1, Empty_i_Pos - 1, Empty_j_Pos);
            else
                DistanceFunction = HamCheckOptimal(CurrNode, IndexAbove_0, Empty_i_Pos - 1, Empty_j_Pos);

            int Move = CurrNode.DepthLevel + 1;
            PuzzleNode t1 = new PuzzleNode(UpBranch, S, Empty_i_Pos - 1, Empty_j_Pos, DistanceFunction, Move, CurrNode);

            int temp = UpBranch[Empty_i_Pos, Empty_j_Pos];
            UpBranch[Empty_i_Pos, Empty_j_Pos] = UpBranch[Empty_i_Pos - 1, Empty_j_Pos];
            UpBranch[Empty_i_Pos - 1, Empty_j_Pos] = temp;

            if ((CurrNode.Parent == null || CurrNode.Parent.Empty_i_Pos != Empty_i_Pos - 1 || CurrNode.Parent.Empty_j_Pos != Empty_j_Pos))
                Children.Add(t1);
        }
        public void DownDirection(bool IsManhattan)
        {
            int[,] DownBranch = new int[S, S];
            for (int i = 0; i < Puzzle.Length; i++)
                DownBranch[i / S, i % S] = Puzzle[i / S, i % S];

            PuzzleNode CurrNode = this;
            int IndexBelow_0 = DownBranch[Empty_i_Pos + 1, Empty_j_Pos];

            int DistanceFunction = 0;
            if (IsManhattan)
                DistanceFunction = ManCheckOptimal(CurrNode, IndexBelow_0 - 1, Empty_i_Pos + 1, Empty_j_Pos);
            else
                DistanceFunction = HamCheckOptimal(CurrNode, IndexBelow_0, Empty_i_Pos + 1, Empty_j_Pos);

            int Move = CurrNode.DepthLevel + 1;
            PuzzleNode t2 = new PuzzleNode(DownBranch, S, Empty_i_Pos + 1, Empty_j_Pos, DistanceFunction, Move, CurrNode);

            int t = DownBranch[Empty_i_Pos + 1, Empty_j_Pos];
            DownBranch[Empty_i_Pos + 1, Empty_j_Pos] = DownBranch[Empty_i_Pos, Empty_j_Pos];
            DownBranch[Empty_i_Pos, Empty_j_Pos] = t;

            if ((CurrNode.Parent == null || CurrNode.Parent.Empty_i_Pos != Empty_i_Pos + 1 || CurrNode.Parent.Empty_j_Pos != Empty_j_Pos))
                Children.Add(t2);
        }
        public void RightDirection(bool IsManhattan)
        {
            int[,] RightBranch = new int[S, S];
            for (int i = 0; i < Puzzle.Length; i++)
                RightBranch[i / S, i % S] = Puzzle[i / S, i % S];

            PuzzleNode CurrNode = this;
            int IndexRight_0 = RightBranch[Empty_i_Pos, Empty_j_Pos + 1];

            int DistanceFunction = 0;
            if (IsManhattan)
                DistanceFunction = ManCheckOptimal(CurrNode, IndexRight_0 - 1, Empty_i_Pos, Empty_j_Pos + 1);
            else
                DistanceFunction = HamCheckOptimal(CurrNode, IndexRight_0, Empty_i_Pos, Empty_j_Pos + 1);

            int Move = CurrNode.DepthLevel + 1;
            PuzzleNode t4 = new PuzzleNode(RightBranch, S, Empty_i_Pos, Empty_j_Pos + 1, DistanceFunction, Move, CurrNode);

            int t = RightBranch[Empty_i_Pos, Empty_j_Pos];
            RightBranch[Empty_i_Pos, Empty_j_Pos] = RightBranch[Empty_i_Pos, Empty_j_Pos + 1];
            RightBranch[Empty_i_Pos, Empty_j_Pos + 1] = t;

            if ((CurrNode.Parent == null || CurrNode.Parent.Empty_i_Pos != Empty_i_Pos || CurrNode.Parent.Empty_j_Pos != Empty_j_Pos + 1))
                Children.Add(t4);
        }
        public void LeftDirection(bool IsManhattan)
        {
            int[,] LeftBranch = new int[S, S];
            for (int i = 0; i < Puzzle.Length; i++)
                LeftBranch[i / S, i % S] = Puzzle[i / S, i % S];

            PuzzleNode CurrNode = this;
            int IndexLeft_0 = LeftBranch[Empty_i_Pos, Empty_j_Pos - 1];

            int DistanceFunction = 0;
            if (IsManhattan)
                DistanceFunction = ManCheckOptimal(CurrNode, IndexLeft_0 - 1, Empty_i_Pos, Empty_j_Pos - 1);
            else
                DistanceFunction = HamCheckOptimal(CurrNode, IndexLeft_0, Empty_i_Pos, Empty_j_Pos - 1);

            int Move = CurrNode.DepthLevel + 1;
            PuzzleNode t3 = new PuzzleNode(LeftBranch, S, Empty_i_Pos, Empty_j_Pos - 1, DistanceFunction, Move, CurrNode);

            int t = LeftBranch[Empty_i_Pos, Empty_j_Pos];
            LeftBranch[Empty_i_Pos, Empty_j_Pos] = LeftBranch[Empty_i_Pos, Empty_j_Pos - 1];
            LeftBranch[Empty_i_Pos, Empty_j_Pos - 1] = t;

            if ((CurrNode.Parent == null || CurrNode.Parent.Empty_i_Pos != Empty_i_Pos || CurrNode.Parent.Empty_j_Pos != Empty_j_Pos - 1))
                Children.Add(t3);
        }
        public void NextMove(bool IsManhattan)
        {
            this.FeasibleMoves();
            if (this.CanMoveUp)
                this.UpDirection(IsManhattan);
            if (this.CanMoveDown)
                this.DownDirection(IsManhattan);
            if (this.CanMoveRight)
                this.RightDirection(IsManhattan);
            if (this.CanMoveLeft)
                this.LeftDirection(IsManhattan);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace N_Puzzle
{
    class PuzzleNode
    {
        public int[,] Puzzle;
        public int[] Puzzle_1D;

        public int S;
        public int DepthLevel;
        public int M_value; // Manhattan
        public int H_value; // Hamming
        public int Empty_i_Pos;
        public int Empty_j_Pos;
        public int Moves = 0;
        public int Move_cost;
        public string Move_dir;
        public string MainKey;
        public PuzzleNode parent;

        public bool CanMoveUp = false;
        public bool CanMoveRight = false;
        public bool CanMoveDown = false;
        public bool CanMoveLeft = false;

        // Main Constructor
        public PuzzleNode(int[,] Puzzle, int S, int Empty_i_Pos, int Empty_j_Pos)
        {
            this.S = S;
            this.Puzzle = new int[S, S];
            this.Puzzle_1D = new int[S*S];

            for (int i = 0; i < this.S*this.S; i++)
            {
                this.Puzzle[i / this.S, i % this.S] = Puzzle[i / this.S, i % this.S];
                this.Puzzle_1D[i] = Puzzle[i / this.S, i % this.S];
                this.MainKey = this.MainKey + Puzzle[i / this.S, i % this.S];
            }

            this.DepthLevel = 0;
            this.Empty_i_Pos = Empty_i_Pos;
            this.Empty_j_Pos = Empty_j_Pos;
            this.Move_dir = "Initial";
            this.parent = null;
            
            this.M_value = Program.ManhattanPriorityDistance(this.Puzzle, this.Puzzle_1D, this.S);
            this.H_value = Program.HammingPriorityDistance(this.Puzzle, this.Puzzle_1D, this.S);
        }
        // Secondry Copy Constructor
        public PuzzleNode(PuzzleNode pn)
        {
            this.S = pn.S;
            this.Puzzle = new int[S, S];
            this.Puzzle_1D = new int[S*S];

            for (int i = 0; i < this.S * this.S; i++)
            {
                this.Puzzle[i / this.S, i % this.S] = pn.Puzzle[i / this.S, i % this.S];
                this.Puzzle_1D[i] = pn.Puzzle[i / this.S, i % this.S];
            }

            this.DepthLevel = pn.DepthLevel + 1;
            this.Empty_i_Pos = pn.Empty_i_Pos;
            this.Empty_j_Pos = pn.Empty_j_Pos;
            this.Move_dir = "To Be Determined";
            this.parent = pn.parent;

            this.M_value = pn.M_value;
            this.H_value = pn.H_value;
        }

        public void ManhattanMin()
        {
            this.Move_cost = this.M_value + this.DepthLevel;
        }
        public void HammingMin()
        {
            this.Move_cost = this.H_value + this.DepthLevel;
        }
        
        public void FeasibleMoves()
        {
            if (this.Empty_i_Pos == 0)
            {
                this.CanMoveDown = true;
                if(this.Empty_j_Pos == 0) { this.CanMoveRight = true; }
                else if(this.Empty_j_Pos == this.S - 1) { this.CanMoveLeft = true; }
                else { this.CanMoveRight = true; this.CanMoveLeft = true; }
            }
            else if (this.Empty_j_Pos == 0)
            {
                this.CanMoveRight = true;
                this.CanMoveUp = true;
                this.CanMoveDown = true;
            }
            else if (this.Empty_i_Pos == this.S - 1)
            {
                this.CanMoveUp = true;
                if (this.Empty_j_Pos == 0) { this.CanMoveRight = true; }
                else if (this.Empty_j_Pos == this.S - 1) { this.CanMoveLeft = true; }
                else { this.CanMoveRight = true; this.CanMoveLeft = true; }
            }
            else if (this.Empty_j_Pos == this.S - 1)
            {
                this.CanMoveLeft = true;
                this.CanMoveUp = true;
                this.CanMoveDown = true;
            }
            else
            {
                this.CanMoveUp = true;
                this.CanMoveRight = true;
                this.CanMoveDown = true;
                this.CanMoveLeft = true;
            }
        }

        public PuzzleNode UpDirection(PuzzleNode pn)
        {
            PuzzleNode UpNode = new PuzzleNode(pn);
            int UpPosition = UpNode.Puzzle[UpNode.Empty_i_Pos - 1, UpNode.Empty_j_Pos];
            UpNode.Puzzle[UpNode.Empty_i_Pos, UpNode.Empty_j_Pos] = UpPosition;
            UpNode.Puzzle[UpNode.Empty_i_Pos - 1, UpNode.Empty_j_Pos] = 0;
            UpNode.Empty_i_Pos--;

            UpNode.Moves++;
            //UpNode.M_value = Program.ManhattanPriorityDistance(UpNode.Puzzle, UpNode.Puzzle_1D, UpNode.S);
            //UpNode.H_value = Program.HammingPriorityDistance(UpNode.Puzzle, UpNode.Puzzle_1D, UpNode.S);
            return UpNode;
        }
        public PuzzleNode RightDirection(PuzzleNode pn)
        {
            PuzzleNode RightNode = new PuzzleNode(pn);
            int RightPosition = RightNode.Puzzle[RightNode.Empty_i_Pos, RightNode.Empty_j_Pos + 1];
            RightNode.Puzzle[RightNode.Empty_i_Pos, RightNode.Empty_j_Pos] = RightPosition;
            RightNode.Puzzle[RightNode.Empty_i_Pos, RightNode.Empty_j_Pos + 1] = 0;
            RightNode.Empty_j_Pos++;

            RightNode.Moves++;
            return RightNode;
        }
        public PuzzleNode DownDirection(PuzzleNode pn)
        {
            PuzzleNode DownNode = new PuzzleNode(pn);
            int DownPosition = DownNode.Puzzle[DownNode.Empty_i_Pos + 1, DownNode.Empty_j_Pos];
            DownNode.Puzzle[DownNode.Empty_i_Pos, DownNode.Empty_j_Pos] = DownPosition;
            DownNode.Puzzle[DownNode.Empty_i_Pos + 1, DownNode.Empty_j_Pos] = 0;
            DownNode.Empty_i_Pos++;

            DownNode.Moves++;
            return DownNode;
        }
        public PuzzleNode LeftDirection(PuzzleNode pn)
        {
            PuzzleNode LeftNode = new PuzzleNode(pn);
            int LeftPosition = LeftNode.Puzzle[LeftNode.Empty_i_Pos, LeftNode.Empty_j_Pos - 1];
            LeftNode.Puzzle[LeftNode.Empty_i_Pos, LeftNode.Empty_j_Pos] = LeftPosition;
            LeftNode.Puzzle[LeftNode.Empty_i_Pos, LeftNode.Empty_j_Pos - 1] = 0;
            LeftNode.Empty_j_Pos--;

            LeftNode.Moves++;
            return LeftNode;
        }
    }
}

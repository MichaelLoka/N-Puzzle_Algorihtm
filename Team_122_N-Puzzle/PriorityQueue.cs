using System;
using System.Collections.Generic;
using System.Text;

namespace Team_122_N_Puzzle
{
    class PriorityQueue
    {
        PuzzleNode[] Combinations;
        int NodeCount = 0;
        public PriorityQueue()
        {
            Combinations = new PuzzleNode[(int)1e7];
        }
        public void Enqueue(PuzzleNode pn)
        {
            NodeCount++;
            Combinations[NodeCount] = pn;
            UpHeapSort();
        }

        public PuzzleNode Dequeue()
        {
            int FirstIndex = 1;
            int LastIndex = NodeCount;
            PuzzleNode First = Combinations[FirstIndex];
            Combinations[FirstIndex] = Combinations[LastIndex];
            NodeCount--;
            DownHeapSort(NodeCount, 1);
            return First;
        }


        void UpHeapSort()
        {
            int Index = NodeCount;
            while (BigParent(Index))
            {
                PuzzleNode temp = Combinations[Index / 2];
                Combinations[Index / 2] = Combinations[Index];
                Combinations[Index] = temp;
                Index /= 2;
            }
        }

        bool BigParent(int Index)
        {
            if (Index > 1)
                return (Combinations[Index / 2].TotalCost >= Combinations[Index].TotalCost);
            return false;
        }

        void DownHeapSort(int N, int Index)
        {
            int Min;
            int LeftChild = Index * 2;
            int RightChild = LeftChild + 1;

            if (BigChild(N, LeftChild, Index))
                Min = LeftChild;
            else
                Min = Index;
            if (BigChild(N, RightChild, Min))
                Min = RightChild;
            if (Index != Min)
            {
                PuzzleNode temp = Combinations[Index];
                Combinations[Index] = Combinations[Min];
                Combinations[Min] = temp;
                DownHeapSort(N, Min);
            }
        }
        bool BigChild(int N, int Child, int Comp)
        {
            if (Child <= N)
                return (Combinations[Child].TotalCost < Combinations[Comp].TotalCost);
            return false;
        }
    }
}

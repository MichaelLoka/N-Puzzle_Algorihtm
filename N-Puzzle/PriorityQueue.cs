using System;
using System.Collections.Generic;
using System.Text;

namespace N_Puzzle
{
    class PriorityQueue
    {
        List<PuzzleNode> Combinations;

        public PriorityQueue()
        {
            this.Combinations = new List<PuzzleNode>();
        }
        public void Enqueue(PuzzleNode pn, char Dist_Func)
        {
            Combinations.Add(pn);
            UpHeapSort(Dist_Func);
        }
        public PuzzleNode Dequeue()
        {
            int FirstIndex = 0;
            int LastIndex = Combinations.Count - 1;
            PuzzleNode First = Combinations[FirstIndex];
            Combinations[FirstIndex] = Combinations[LastIndex];
            Combinations.RemoveAt(LastIndex);
            //DownHeapSort();
            return First;
        }
        public bool Empty()
        {
            return (Combinations.Count == 0);
        }
        public int Size()
        {
            return Combinations.Count;
        }
        public void UpHeapSort(char Dist_Func)
        {
            int ChildIndex = Combinations.Count - 1;
            while (ChildIndex > 0)
            {
                int ParentIndex = (ChildIndex - 1) / 2;
                if (Dist_Func == 'M') // Manhattan
                {
                    if (Combinations[ChildIndex].M_value >= Combinations[ParentIndex].M_value
                        || Combinations[ChildIndex].Move_cost >= Combinations[ParentIndex].Move_cost)
                        break;
                    PuzzleNode temp = Combinations[ChildIndex];
                    Combinations[ChildIndex] = Combinations[ParentIndex];
                    Combinations[ParentIndex] = temp;
                    ChildIndex = ParentIndex;
                }
                else if (Dist_Func == 'H') // Hamming
                {
                    if (Combinations[ChildIndex].H_value >= Combinations[ParentIndex].H_value
                        || Combinations[ChildIndex].Move_cost >= Combinations[ParentIndex].Move_cost)
                        break;
                    PuzzleNode temp = Combinations[ChildIndex];
                    Combinations[ChildIndex] = Combinations[ParentIndex];
                    Combinations[ParentIndex] = temp;
                    ChildIndex = ParentIndex;
                }
            }
        }
    }
}

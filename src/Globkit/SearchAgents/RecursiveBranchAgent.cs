using System.Collections.Generic;

namespace Globkit.SearchAgents
{
    internal class RecursiveBranchAgent : SearchAgent
    {
        public RecursiveBranchAgent(string expression, ISearchTree searchTree) : base(expression, searchTree)
        {
        }

        protected override void PerformSearch(string path, ICollection<string> results)
        {
            if (Next == null) return;

            Next.Search(path, results);
            foreach (var dir in SearchTree.GetBranches(path))
            {
                var nextDir = SearchTree.CombinePaths(path, dir);
                PerformSearch(nextDir, results);
            }
        }
    }
}
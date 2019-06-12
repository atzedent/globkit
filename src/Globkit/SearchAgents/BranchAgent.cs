using System.Collections.Generic;

namespace Globkit.SearchAgents
{
    internal class BranchAgent : SearchAgent
    {
        public BranchAgent(string expression, ISearchTree searchTree) : base(expression, searchTree) { }
        protected override void PerformSearch(string path, ICollection<string> results)
        {
            if (Next == null) return;

            foreach (var dir in SearchTree.GetBranches(path, Expression))
                Next.Search(SearchTree.CombinePaths(path, dir), results);
        }
    }
}
using System.Collections.Generic;

namespace Globkit.SearchAgents
{
    internal class BranchAgent : SearchAgent
    {
        public BranchAgent(string expression, ISearchTree searchTree) : base(expression, searchTree) { }
        protected override IEnumerable<string> PerformSearch(string path)
        {
            if (Next != null)
            {
                foreach (var dir in SearchTree.GetBranches(path, Expression))
                    foreach (var d in Next.Search(SearchTree.CombinePaths(path, dir)))
                        yield return d;
            }
        }
    }
}
using System.Collections.Generic;

namespace Globkit.SearchAgents
{
    internal class RecursiveBranchAgent : SearchAgent
    {
        public RecursiveBranchAgent(string expression, ISearchTree searchTree) : base(expression, searchTree)
        {
        }

        protected override IEnumerable<string> PerformSearch(string path)
        {
            if (Next != null)
            {
                foreach (var dir in Next.Search(path))
                    yield return dir;
                foreach (var dir in SearchTree.GetBranches(path))
                {
                    var nextDir = SearchTree.CombinePaths(path, dir);
                    foreach (var d in PerformSearch(nextDir))
                        yield return d;
                }
            }
        }
    }
}
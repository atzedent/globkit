using System.Collections.Generic;

namespace Globkit.SearchAgents
{
    internal class LeafAgent : SearchAgent
    {
        public LeafAgent(string expression, ISearchTree searchTree) : base(expression, searchTree) { }
        protected override IEnumerable<string> PerformSearch(string path)
        {
            foreach (var file in SearchTree.GetLeaves(path, Expression))
                yield return file;
        }
    }
}
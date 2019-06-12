using System.Collections.Generic;

namespace Globkit.SearchAgents
{
    internal class LeafAgent : SearchAgent
    {
        public LeafAgent(string expression, ISearchTree searchTree) : base(expression, searchTree) { }
        protected override void PerformSearch(string path, ICollection<string> results)
        {
            foreach (var file in SearchTree.GetLeaves(path, Expression))
                results.Add(file);
        }
    }
}
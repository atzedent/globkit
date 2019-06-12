using System.Collections.Generic;

namespace Globkit.SearchAgents
{
    internal class NoSearchAgent : SearchAgent
    {
        public NoSearchAgent(string expression, ISearchTree searchTree) : base(expression, searchTree) { }
        protected override void PerformSearch(string path, ICollection<string> results) { }
    }
}
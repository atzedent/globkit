using System.Linq;
using System.Collections.Generic;

namespace Globkit.SearchAgents
{
    internal class NoSearchAgent : SearchAgent
    {
        public NoSearchAgent(string expression, ISearchTree searchTree) : base(expression, searchTree) { }
        protected override IEnumerable<string> PerformSearch(string path) { return Enumerable.Empty<string>(); }
    }
}
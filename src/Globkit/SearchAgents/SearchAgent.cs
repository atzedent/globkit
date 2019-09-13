using System.Collections.Generic;

namespace Globkit.SearchAgents
{
    public abstract class SearchAgent
    {
        public SearchAgent Next { get; set; }
        protected string Expression { get; }
        protected ISearchTree SearchTree { get; }
        protected SearchAgent(string expression, ISearchTree searchTree)
        {
            Expression = expression;
            SearchTree = searchTree;
        }

        public IEnumerable<string> Search(string path)
        {
            return PerformSearch(path);
        }

        protected abstract IEnumerable<string> PerformSearch(string path);
    }
}
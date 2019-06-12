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

        public void Search(string path, ICollection<string> results)
        {
            PerformSearch(path, results);
        }

        protected abstract void PerformSearch(string path, ICollection<string> results);
    }
}
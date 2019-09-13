using Globkit.SearchAgents;
using System.Collections.Generic;
using System.Linq;

namespace Globkit
{
    public class Search
    {
        private readonly ISearchTree _searchTree;

        public Search(ISearchTree searchTree)
        {
            _searchTree = searchTree;
        }

        public IEnumerable<string> FindLeaves(string pattern)
        {
            var roots = GetRoots(pattern).ToList();
            foreach (var root in roots)
            {
                var startPattern = pattern.StartsWith(root)
                    ? pattern.Substring(root.Length)
                    : pattern;
                var agent = BuildSearch(startPattern);
                foreach (var result in agent.Search(root))
                    yield return result;
            }
        }

        private IEnumerable<string> GetRoots(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return Enumerable.Empty<string>();
            if (!_searchTree.IsPathRooted(pattern))
                return _searchTree.GetTreeRoots();

            var root = _searchTree.GetPathRoot(pattern);
            var p = pattern.Substring(root.Length);
            var path = GetLongestPath(p, root);

            return new[] { path };
        }

        private string GetLongestPath(string path, string root = "")
        {
            while (true)
            {
                if (string.IsNullOrEmpty(path))
                    return root;

                var parts = path.Trim(_searchTree.PathSeparator).Split(_searchTree.PathSeparator);
                var topDir = parts.FirstOrDefault();
                var curDir = $"{root}{topDir}{_searchTree.PathSeparator}";

                if (!_searchTree.BranchExists(curDir)
                   || string.IsNullOrEmpty(topDir))
                    return root;

                path = path.Substring(topDir.Length + 1);
                root = curDir;
            }
        }

        private SearchAgent BuildSearch(string pattern)
        {
            var expressions = BuildExpressions(pattern);
            var searchAgents = expressions.Select(GetSearchAgent).ToArray();

            return Enumerable.Reverse(searchAgents).Aggregate((a, b) =>
            {
                b.Next = a;
                return b;
            });
        }

        private SearchAgent GetSearchAgent(string expression)
        {
            if (IsRecursiveBranchExpression(expression))
                return new RecursiveBranchAgent(expression.Trim(_searchTree.PathSeparator), _searchTree);
            if (IsBranchExpression(expression))
                return new BranchAgent(expression.Trim(_searchTree.PathSeparator), _searchTree);
            if (IsLeafExpression(expression))
                return new LeafAgent(expression.Trim(_searchTree.PathSeparator), _searchTree);

            return new NoSearchAgent(expression, _searchTree);
        }

        private IEnumerable<string> BuildExpressions(string pattern)
        {
            var expressions = new List<string>();
            for (var index = pattern.IndexOf(_searchTree.PathSeparator);
                index >= 0;
                index = pattern.IndexOf(_searchTree.PathSeparator))
            {
                expressions.Add(pattern.Substring(0, index + 1));
                pattern = pattern.Substring(index + 1);
            }

            expressions.Add(pattern);
            return expressions;
        }

        private bool IsRecursiveBranchExpression(string expression)
        {
            var recPattern = $"**{_searchTree.PathSeparator}";
            return expression == recPattern;
        }

        private bool IsBranchExpression(string expression)
        {
            return expression.IndexOf(_searchTree.PathSeparator) > -1;
        }

        private bool IsLeafExpression(string expression)
        {
            return expression.IndexOf(_searchTree.PathSeparator) < 0;
        }
    }
}
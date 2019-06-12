using Globkit.SearchAgents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var results = new List<IEnumerable<string>>();
            Parallel.ForEach(roots, root =>
            {
                var r = new List<string>();
                results.Add(r);
                var startPattern = pattern.StartsWith(root)
                    ? pattern.Substring(root.Length)
                    : pattern;
                var agent = BuildSearch(startPattern);
                agent.Search(root, r);
            });
            return results.SelectMany(r => r);
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
            var searchExpressions = expressions.Select(GetSearchExpressionFor).ToList();

            return Enumerable.Reverse(searchExpressions).Aggregate((a, b) =>
            {
                b.Next = a;
                return b;
            });
        }

        private SearchAgent GetSearchExpressionFor(string expression)
        {
            if (IsRecursiveDirectoryExpression(expression))
                return new RecursiveBranchAgent(expression.Trim(_searchTree.PathSeparator), _searchTree);
            if (IsDirectoryExpression(expression))
                return new BranchAgent(expression.Trim(_searchTree.PathSeparator), _searchTree);
            if (IsFileExpression(expression))
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

        private bool IsRecursiveDirectoryExpression(string expression)
        {
            var recPattern = $"**{_searchTree.PathSeparator}";
            return expression == recPattern;
        }

        private bool IsDirectoryExpression(string expression)
        {
            return expression.IndexOf(_searchTree.PathSeparator) > -1;
        }

        private bool IsFileExpression(string expression)
        {
            return expression.IndexOf(_searchTree.PathSeparator) < 0;
        }
    }
}
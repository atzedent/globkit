using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Globkit.Tests
{
    public class TestSearchTree : ISearchTree
    {
        public List<Branch> Roots { get; }
        public char PathSeparator { get; }
        private readonly Dictionary<string, Branch> _branches = new Dictionary<string, Branch>();

        public TestSearchTree(params string[] leaves) : this(new[] { "/" }, leaves)
        {
        }

        public TestSearchTree(IEnumerable<string> roots, params string[] leaves) : this('/', roots, leaves)
        {
        }

        public TestSearchTree(char pathSeparator, IEnumerable<string> roots, params string[] leaves)
        {
            PathSeparator = pathSeparator;
            Roots = roots.Select(name => new Branch(name)).ToList();
            Build(leaves);
        }

        public bool BranchExists(string path)
        {
            return _branches.TryGetValue(path, out _)
                   || _branches.TryGetValue(path.Substring(0, path.LastIndexOf(PathSeparator)), out _);
        }

        public bool IsPathRooted(string path)
        {
            return !string.IsNullOrEmpty(path)
                   && Roots.Any(r => path.StartsWith(r.Name));
        }

        public string GetPathRoot(string path)
        {
            return path.Substring(0, 1 + path.IndexOf(PathSeparator));
        }

        public string CombinePaths(string first, string second)
        {
            if (second.Length == 0)
            {
                return first;
            }

            if (first.Length == 0)
            {
                return second;
            }

            if (IsPathRooted(second))
            {
                return second;
            }

            var c = first[first.Length - 1];
            if (c != PathSeparator)
            {
                return $"{first}{PathSeparator}{second}";
            }

            return first + second;
        }

        public IEnumerable<string> GetBranches(string path)
        {
            var exists = TryGetBranch(path, out var branch);
            return !exists
                ? Enumerable.Empty<string>()
                : branch.Branches.Select(b => CombinePaths(path, b.Name));
        }

        public IEnumerable<string> GetBranches(string path, string filter)
        {
            var exists = TryGetBranch(path, out var branch);
            if (!exists) return Enumerable.Empty<string>();
            var result = new List<string>();
            foreach (var b in branch.Branches)
                if (Regex.IsMatch(b.Name, Mask(filter)))
                    result.Add(CombinePaths(path, b.Name));
            return result;
        }

        public IEnumerable<string> GetLeaves(string path)
        {
            var exists = TryGetBranch(path, out var branch);
            return !exists
                ? Enumerable.Empty<string>()
                : branch.Leaves.Select(l => CombinePaths(path, l.Name));
        }

        public IEnumerable<string> GetLeaves(string path, string filter)
        {
            var exists = TryGetBranch(path, out var branch);
            if (!exists) return Enumerable.Empty<string>();
            var result = new List<string>();
            foreach (var l in branch.Leaves)
                if (Regex.IsMatch(l.Name, Mask(filter)))
                    result.Add(CombinePaths(path, l.Name));
            return result;
        }

        public IEnumerable<string> GetTreeRoots() => Roots.Select(r => r.Name);

        private bool TryGetBranch(string path, out Branch branch)
        {
            var p = path.Length > 0 && path.LastIndexOf(PathSeparator) == path.Length - 1
                ? path.Substring(0, path.Length - 1)
                : path;

            var exists = _branches.TryGetValue(p, out var b);

            branch = b;
            return exists;
        }

        private void Build(params string[] leaves)
        {
            foreach (var leaf in leaves)
            {
                foreach (var root in Roots)
                {
                    if (!leaf.StartsWith(root.Name)) continue;
                    var path = leaf.Substring(root.Name.Length);
                    var parts = path.Split(PathSeparator).SkipWhile(string.IsNullOrEmpty).ToArray();
                    BuildPath(root, parts);
                }
            }
        }

        private void BuildPath(Branch root, IReadOnlyCollection<string> parts)
        {
            var head = root;
            var path = root.Name;
            if (!_branches.TryAdd(path, root))
                head = _branches[path];
            foreach (var part in parts.Take(parts.Count - 1))
            {
                var branch = new Branch(part);
                path = CombinePaths(path, branch.Name);
                if (_branches.TryAdd(path, branch))
                {
                    head.Branches.Add(branch);
                    head = branch;
                }
                else
                {
                    head = _branches[path];
                }
            }

            head.Leaves.Add(new Leaf(parts.Last()));
        }

        private static string Mask(string value) => $"^{Regex.Escape(value).Replace("\\*", ".*")}$";
    }

    public abstract class TreeItem
    {
        public string Name { get; }

        protected TreeItem(string name)
        {
            Name = name;
        }
    }

    public class Branch : TreeItem
    {
        public List<Branch> Branches { get; }
        public List<Leaf> Leaves { get; }

        public Branch(string name) : base(name)
        {
            Branches = new List<Branch>();
            Leaves = new List<Leaf>();
        }
    }

    public class Leaf : TreeItem
    {
        public Leaf(string name) : base(name)
        {
        }
    }
}
using System.Collections.Generic;

namespace Globkit
{
    public interface ISearchTree
    {
        char PathSeparator { get; }
        bool BranchExists(string path);
        bool IsPathRooted(string path);
        string GetPathRoot(string path);
        string CombinePaths(string first, string second);
        IEnumerable<string> GetBranches(string path);
        IEnumerable<string> GetBranches(string path, string filter);
        IEnumerable<string> GetLeaves(string path);
        IEnumerable<string> GetLeaves(string path, string filter);
        IEnumerable<string> GetTreeRoots();
    }
}
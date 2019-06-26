[![Build Status](https://dev.azure.com/cry4hurrle/cry4hurrle/_apis/build/status/atzedent.globkit?branchName=master)](https://dev.azure.com/cry4hurrle/cry4hurrle/_build/latest?definitionId=1&branchName=master)

# Globkit

Search any tree-like data structure with wildcard and globbing patterns.

## About The Project

Simple, manageable code helps to understand the solution to the problem step by step. In this case, it is to understand globbing. Incidentally, it allows a fairly quick search in the file system.
The code is deliberately kept simple so as not to distract from the actual goal.

### Built With

* [C#](https://github.com/dotnet/csharplang)
* [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/)

## Getting started

Either [clone the repo](#Clone-The-Repo) and add the project to your code-base or [reference the nuget package](#Reference-The-NuGet-Package).

### Prerequisites

The project uses the target framework netstandard2.0. Make sure you have a [corresponding SDK](https://dotnet.microsoft.com/download/dotnet-core/2.0) installed.

### Clone The Repo

```sh
git clone https://github.com/atzedent/globkit.git
```

### Reference The NuGet Package

```sh
dotnet add package Globkit --version 1.0.1
```

## Usage

First you need an instance of your search tree. As a simple example, you can use the one shown below for the file system. With this create a new instance of the search. Now you are ready to search your tree with wildcard and globbing pattens.

### The Thing With The Pathseparator

The pathseparator is a constant character of the search tree's implementation. It is up to you to define yours for your implementation of your search tree. See the TestSearchTree in Globkit.Tests for a malleable version of such an implementation.

### Examples

If you use the below FileSystemSearchTree then `/**/*.log` on UNIX like systems searches recursively in all directories found under `/`.
`/var/log/*.log` searches flat in only `/var/log/` for files with the extension `.log`.

`**\*.png` scans all Windows drives and searches for PNG files.

### File System Search

That's all it takes to search the file system. 

```csharp
void Main() {
	var searchTree = new FilesystemSearchTree();
	var search = new Search(searchTree);
	var result = search.FindLeaves(Console.ReadLine());
	foreach (var file in result)
		Console.WriteLine(file);
}
class FilesystemSearchTree : ISearchTree {
	public char PathSeparator => Path.DirectorySeparatorChar;
	public bool BranchExists(string path) {
		try { return Directory.Exists(path); }
		catch { return false; }
	}
	public string CombinePaths(string first, string second) {
		return Path.Combine(first, second);
	}
	public IEnumerable<string> GetBranches(string path){
		try { return Directory.EnumerateDirectories(path); }
		catch { return Enumerable.Empty<string>(); }
	}
	public IEnumerable<string> GetBranches(string path, string filter) {
		try { return Directory.EnumerateDirectories(path, filter); }
		catch { return Enumerable.Empty<string>(); }
	}
	public IEnumerable<string> GetLeaves(string path) {
		try { return Directory.EnumerateFiles(path); }
		catch { return Enumerable.Empty<string>(); }
	}
	public IEnumerable<string> GetLeaves(string path, string filter) {
		try { return Directory.EnumerateFiles(path, filter); }
		catch { return Enumerable.Empty<string>(); }
	}
	public string GetPathRoot(string path) {
		return Path.GetPathRoot(path);
	}
	public IEnumerable<string> GetTreeRoots() {
		return Environment.GetLogicalDrives();
	}
	public bool IsPathRooted(string path) {
		return Path.IsPathRooted(path);
	}
}
```

## Contributing

Contributions are what make the open source community such an amazing place to be, learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

Distributed under the MIT License. See `LICENSE` for more information.

## Contact

Matthias Hurrle - [@SOFTWARETOGO](https://twitter.com/softwaretogo) - cry4hurrle(at)gmail.com

Project Link: [https://github.com/atzedent/globkit](https://github.com/atzedent/globkit)

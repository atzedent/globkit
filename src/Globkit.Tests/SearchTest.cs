using Xunit;

namespace Globkit.Tests
{
    public class SearchTest
    {
        [Fact]
        public void FindLeaves_On_Leaf_Exists_Returns_One_Result()
        {
            const string leafPath = "/branch/branch/leaf";
            var searchTree = new TestSearchTree(leafPath);
            var search = new Search(searchTree);
            var result = search.FindLeaves(leafPath);
            Assert.Collection(result, s => Assert.Equal(leafPath, s));
        }

        [Fact]
        public void FindLeaves_On_Leaf_Does_Not_Exist_Returns_Empty_Result()
        {
            const string leafPath = "/branch/branch/leaf";
            var searchTree = new TestSearchTree(leafPath);
            var search = new Search(searchTree);
            var result = search.FindLeaves("/branch/leaf");
            Assert.Empty(result);
        }

        [Fact]
        public void Find_Leaves_On_Leaf_Exists_With_Custom_Path_Separator_Returns_One_Result()
        {
            const char pathSeparator = '#';
            const string leafPath = "WALTER#branch#branch#leaf";
            var searchTree = new TestSearchTree(pathSeparator, new[] { "WALTER" }, leafPath);
            var search = new Search(searchTree);
            var result = search.FindLeaves(leafPath);
            Assert.Collection(result, s => Assert.Equal(leafPath, s));
        }

        [Fact]
        public void FindLeaves_On_Leaf_Exists_On_One_Root_Returns_One_Result()
        {
            const string leafPath = "HUGO/a/b/c";
            var searchTree = new TestSearchTree(new[] { "HUGO", "BERTA" }, leafPath);
            var search = new Search(searchTree);
            var result = search.FindLeaves("**/a/b/c");
            Assert.Collection(result, s => Assert.Equal(leafPath, s));
        }

        [Fact]
        public void FindLeaves_On_Leaf_Exists_On_Two_Roots_Returns_Two_Results()
        {
            const string leafPath1 = "HUGO/a/b/c";
            const string leafPath2 = "BERTA/a/b/c";
            const string leafPath3 = "BERTA/a2/b/c";
            var searchTree = new TestSearchTree(new[] { "HUGO", "BERTA" }, leafPath1, leafPath2, leafPath3);
            var search = new Search(searchTree);
            var result = search.FindLeaves("**/a/*b*/*c*");
            Assert.Collection(result,
                s => Assert.Equal(leafPath1, s),
                s => Assert.Equal(leafPath2, s));
        }

        [Fact]
        public void FindLeaves_On_Leaf_Exists_On_One_Branch_Returns_One_Result()
        {
            const string leafPath = "/a/b/c";
            var searchTree = new TestSearchTree(leafPath);
            var search = new Search(searchTree);
            var result = search.FindLeaves("/a/**/c");
            Assert.Collection(result, s => Assert.Equal(leafPath, s));
        }

        [Fact]
        public void FindLeaves_On_Leaf_Exists_On_Two_Branches_Returns_Two_Results()
        {
            const string leafPath1 = "/a/b/c";
            const string leafPath2 = "/a/y/c";
            var searchTree = new TestSearchTree(leafPath1, leafPath2);
            var search = new Search(searchTree);
            var result = search.FindLeaves("/a/**/c");
            Assert.Collection(result,
                s => Assert.Equal(leafPath1, s),
                s => Assert.Equal(leafPath2, s));
        }

        [Fact]
        public void FindLeaves_On_Leaf_Exists_On_One_Branch_With_Search_Pattern_Variation_Returns_One_Result()
        {
            const string leafPath = "/a/b/c";
            var searchTree = new TestSearchTree(leafPath);
            var search = new Search(searchTree);
            var result = search.FindLeaves("/a/b/**/c");
            Assert.Collection(result, s => Assert.Equal(leafPath, s));
        }
    }
}
namespace HiriseLib
{
    public static class Protocol
    {
        internal const char FolderSeparator = ':';
        internal const char FolderItemSeparator = ':';
        internal const string FolderSeparatorString = ":";

        internal const string UserNamespace = "user" + FolderSeparatorString;
        internal const string TreeNamespace = "tree" + FolderSeparatorString;

        public static string CombineFolders(string folder1, string folder2) => string.IsNullOrEmpty(folder1) ? folder2 : folder1 + FolderSeparatorString + folder2;

        public static string CombineFolderAndItem(string folder, string itemName) => folder + FolderItemSeparator + itemName;

        public static string[] SplitFolders(string path) => path.Split(FolderSeparator);
        public static void SplitFolderAndItems(string itemPath, out string[] folders, out string itemName)
        {
            if (itemPath.StartsWith(TreeNamespace))
            {
                itemPath = itemPath[TreeNamespace.Length..];
            }
            int folderItemSeparatorIndex = itemPath.LastIndexOf(FolderItemSeparator);
            if (folderItemSeparatorIndex == -1)
            {
                itemName = "";
                folders = SplitFolders(itemPath);
            }
            else
            {
                itemName = itemPath[folderItemSeparatorIndex..];
                folders = SplitFolders(itemPath[..folderItemSeparatorIndex]);
            }
        }
    }
}

﻿using ShUtilities.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HiriseLib
{
    public static class Protocol
    {
        internal const char NewLine = '\n';
        internal const string NewLineString = "\n";

        internal const char FolderSeparator = '\\';
        internal const string FolderSeparatorString = "\\";

        internal const char FolderItemSeparator = '.';

        public static string CombineFolders(string folder1, string folder2) => string.IsNullOrEmpty(folder1) ? folder2 : folder1 + FolderSeparatorString + folder2;

        public static string CombineFolderAndItem(string folder, string itemName) => folder + FolderItemSeparator + itemName;

        public static string[] SplitFolders(string path) => path.Split(FolderSeparator);
        public static void SplitFolderAndItems(string itemPath, out string[] folders, out string itemName)
        {
            int folderItemSeparatorIndex = itemPath.LastIndexOf(FolderItemSeparator);
            if (folderItemSeparatorIndex == -1)
            {
                itemName = "";
                folders = SplitFolders(itemPath);
            }
            else
            {
                itemName = itemPath[(folderItemSeparatorIndex+1)..];
                folders = SplitFolders(itemPath[..folderItemSeparatorIndex]);
            }
        }

        public static string ElementsToStringList<T>(IEnumerable<T> elements) where T : IPathElement => elements.Select(x => x.Path).ToDelimitedString(NewLineString);

    }
}

﻿namespace HiriseLib.Tree
{
    internal class Item: TreeElement, IItem
    {
        public Item(string name, Folder parentFolder, ITreeConnection connection): base(name, parentFolder, connection)
        {
        }

        public override string Path => Protocol.CombineFolderAndItem(_parentFolder.Path, Name);
        
        public string Data { get; set; }

        public void Store(string data, IClientSession clientSession) => _connection.Store(this, data, clientSession);
    }
}
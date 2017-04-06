using UnityEngine;
using System.Collections.Generic;
using System;

public sealed class TableManager 
{
    public static void Load()
    {       
        LoadTable<LevelTable>("LevelTable");
        
    }

    public static T LoadTable<T>(string tableName)
    {
        TextAsset table = GetTableAsset(tableName);
        T t = TableSerializer.Derialize<T>(table.bytes);
        return t;

    }


    public static void Save()
    {

    }

    public static TextAsset GetTableAsset(string tableName)
    {
        string path = String.Format("{0}/{1}", "TableData", tableName);
        return Resources.Load<TextAsset>(path);
    }
}
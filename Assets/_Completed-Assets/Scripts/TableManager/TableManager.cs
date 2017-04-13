using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public sealed class TableManager 
{
    //public static void Load()
    //{       
    //    LoadTable<LevelTable>("LevelTable");

    //}

    //public static T LoadTable<T>(string tableName)
    //{
    //    TextAsset table = GetTableAsset(tableName);
    //    T t = TableSerializer.Derialize<T>(table.bytes);
    //    return t;

    //}


    //public static void Save()
    //{

    //}

    //public static TextAsset GetTableAsset(string tableName)
    //{
    //    string path = String.Format("{0}/{1}", "TableData", tableName);
    //    return Resources.Load<TextAsset>(path);
    //}

    public static T Load<T>(string loadFileName)
    {
        string savePath = Util.GetDataPath(loadFileName + ".xml");
        StreamReader sr = null;

        try
        {
            sr = new StreamReader(savePath);
            T data = TableSerializer.Derialize<T>(sr);
            sr.Close();
            sr = null;
            return data;
        }
        catch (System.Exception e)
        {
            if (null != sr)
            {
                sr.Close();
            }          
            Debug.Log("LocalSaveData Load Exception : " + e.Message);
            return default(T);
        }
    }

    public static void Save<T>(string saveFileName,T data)
    {      
        string savePath = Util.GetDataPath(saveFileName + ".xml");
        System.IO.StreamWriter sw = null;
        sw = new StreamWriter(savePath);
        TableSerializer.Serialize<T>(sw, data);
        sw.Close();
    }


}
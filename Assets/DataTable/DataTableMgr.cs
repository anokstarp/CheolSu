using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;

public static class DataTableMgr
{
    private static Dictionary<System.Type, DataTable> tables = new Dictionary<System.Type, DataTable>();

    static DataTableMgr()
    {
        tables.Clear();
        //static ������ ó�� ������ �� �����

        var stringTable = new MonsterTable();
        tables.Add(typeof(MonsterTable), stringTable);
    }

    public static T GetTable<T>() where T : DataTable
    {
        var id = typeof(T);
        if (!tables.ContainsKey(id))
        {
            return null;
        }
        return tables[id] as T;
    }
}

using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class DropData
{
    public string Monster_Name { get; set; }
    public string Monster_ID { get; set; }
    public int Drop_ID { get; set; }
    public int Monster_EXP { get; set; }
    public int Monster_Gold { get; set; }
    public int Monster_DGP { get; set; }
}

public class DropTable : DataTable
{
    //protected List<DropData> m_DropTableList = new List<DropData>();
    protected Dictionary<int, DropData> m_DropTableDictionary = new Dictionary<int, DropData>();

    public DropTable()
    {
        path = "DropTable";
        Load();
    }

    public override void Load()
    {
        var csvData = Resources.Load<TextAsset>(path);

        TextReader reader = new StringReader(csvData.text);

        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        //csvConfiguration.HasHeaderRecord = true; // CSV ���Ͽ� ����� �ִ� ���

        var csv = new CsvReader(reader, csvConfiguration);

        try
        {
            var records = csv.GetRecords<DropData>();

            foreach (var record in records)
            {
                var temp = new DropData();
                temp = record;
                m_DropTableDictionary.Add(temp.Drop_ID, temp);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError("csv �ε� ����");
        }
    }

    public DropData GetMonsterData(int ID)
    {
        var data = m_DropTableDictionary[ID];
        return data;
    }
}

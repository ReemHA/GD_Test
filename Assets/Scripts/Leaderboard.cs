using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public struct Record
{
    public string name;
    public int coins;
}

[Serializable]
public class LeaderboardData
{
    public int maxPlayersCount;
    public List<Record> records;
    public LeaderboardData(int maxPlayersCount)
    {
        this.maxPlayersCount = maxPlayersCount;
    }
}

public class Leaderboard
{
    public LeaderboardData leaderboardData;
    private int currNonEmptyIndex = -1;
    private string path = Path.Combine(Directory.GetCurrentDirectory(), "Leaderboard.json");

    public Leaderboard()
    {
        leaderboardData = new LeaderboardData(10);
        ReadFromFile();
    }

    public void AddToRecords(Record record)
    {
        // TODO: check array length and before it check highest score.
        // check if it's the first record
        if (currNonEmptyIndex < 0)
        {
            currNonEmptyIndex++;
            leaderboardData.records.Add(record);
        }
        else
        {
            // check if score is higher than the lowest score
            AddToList(record);
            //SortDesc();
        }
    }

    private void AddToList(Record record)
    {
        for (int i = currNonEmptyIndex; i >= 0; i--)
        {
            if (leaderboardData.records[i].coins <= record.coins)
            {
                if (i == 0)
                {
                    InsertToRecords(record, 0);
                }
            }
            else
            {
                InsertToRecords(record, i + 1);
                break;
            }
        }
    }

    private void InsertToRecords(Record record, int index)
    {
        leaderboardData.records.Insert(index, record);
        if (leaderboardData.records.Count > leaderboardData.maxPlayersCount)
        {
            leaderboardData.records.RemoveAt(leaderboardData.records.Count - 1);
        }
        currNonEmptyIndex = leaderboardData.records.Count - 1;
    }

    private void SortDesc()
    {
        throw new NotImplementedException();
    }

    public void SaveToFile()
    {
        string data = JsonUtility.ToJson(leaderboardData);
        File.WriteAllText(path, data);
    }

    public void ReadFromFile()
    {
        try
        {
            string jsonData = File.ReadAllText(path);
            leaderboardData = JsonUtility.FromJson<LeaderboardData>(jsonData);
            currNonEmptyIndex = leaderboardData.records.Count - 1;
        }
        catch (Exception)
        {
            leaderboardData.records = new List<Record>(leaderboardData.maxPlayersCount);
        }
    }
}

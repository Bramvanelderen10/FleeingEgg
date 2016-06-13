using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.SqliteClient;
using System;

public class DatabaseManager : MonoBehaviour
{

    private string _constr = "URI=file:FlyingLeaderboards.db";
    private string _constr_android = "URI=file:" + Application.persistentDataPath + "/" + "FlyingLeaderboards.db";
    private IDbConnection _connection;
    private IDbCommand _command;
    private IDataReader _dbr;
    private string sql;

    // Use this for initialization
    void Awake()
    {
        OpenConnection();
        sql = "CREATE TABLE IF NOT EXISTS highscores (name VARCHAR(20), score INT)";
        _command.CommandText = sql;
        _command.ExecuteNonQuery();
        CloseConnection();   
    }

    public void OpenConnection()
    {
#if UNITY_ANDROID
        _connection = new SqliteConnection(_constr_android);
#else
        _connection = new SqliteConnection(_constr);
#endif
        _connection.Open();
        _command = _connection.CreateCommand();
    }

    public void CloseConnection()
    {
        _command.Dispose();
        _command = null;
        _connection.Close();
        _connection = null;
    }

    public bool InsertScore(string name, int score)
    {
        OpenConnection();

        sql = "INSERT INTO highscores (name, score) VALUES ('" + name + "', " + score + ")";
        _command.CommandText = sql;
        _command.ExecuteNonQuery();

        CloseConnection();

        return true;
    }

    public Dictionary<string, int> RetrieveTopScores(int count)
    {
        OpenConnection();

        Dictionary<string, int> dc = new Dictionary<string, int>();
        sql = "select * from highscores order by score desc limit 10";
        _command.CommandText = sql;
        IDataReader _reader = _command.ExecuteReader();
        while (_reader.Read())
            dc.Add(_reader["name"].ToString(), Int32.Parse(_reader["score"].ToString()));
        _reader.Close();
        _reader = null;

        CloseConnection();

        return dc;
    }
}

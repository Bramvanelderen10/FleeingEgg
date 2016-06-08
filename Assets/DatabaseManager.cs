using UnityEngine;
using System.Collections;
using System.Data;
using Mono.Data.SqliteClient;

public class DatabaseManager : MonoBehaviour
{

    private string _constr = "URI=file:FleeingLeaderboards.db";
    private IDbConnection _connection;
    private IDbCommand _command;
    private IDataReader _dbr;

    // Use this for initialization
    void Awake()
    {
        _connection = new SqliteConnection(_constr);
        _connection.Open();
        _command = _connection.CreateCommand();
        string sql;
        sql = "CREATE TABLE IF NOT EXISTS highscores (name VARCHAR(20), score INT)";
        _command.CommandText = sql;
        _command.ExecuteNonQuery();

        //sql = "INSERT INTO highscores (name, score) VALUES ('test', 0)";
        //_command.CommandText = sql;
        //_command.ExecuteNonQuery();


        sql = "select * from highscores order by score desc";
        _command.CommandText = sql;
        IDataReader _reader = _command.ExecuteReader();
        while (_reader.Read())
            print("****** Name: " + _reader["name"] + "\tScore: " + _reader["score"]);

        _reader.Close();
        _reader = null;
        _command.Dispose();
        _command = null;
        _connection.Close();
        _connection = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

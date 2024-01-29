using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace PracaDomowa.SQLITE
{
    internal class SongRepository
    {
        private readonly string connectionString;

        public SongRepository(string connectionString)
        {
            this.connectionString = connectionString;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Open();

                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Songs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Artist TEXT NOT NULL,
                    Year INTEGER NOT NULL,
                    DurationInSeconds REAL NOT NULL
                )";
                dbConnection.Execute(createTableQuery);
            }
        }

        public void CreateSong(Song song)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("INSERT INTO Songs (Title, Artist, Year, DurationInSeconds) VALUES (@Title, @Artist, @Year, @DurationInSeconds)", song);
            }
        }

        public IEnumerable<Song> ReadSongs()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Open();
                return dbConnection.Query<Song>("SELECT * FROM Songs");
            }
        }

        public void UpdateSong(int id, Song updatedSong)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("UPDATE Songs SET Title = @Title, Artist = @Artist, Year = @Year, DurationInSeconds = @DurationInSeconds WHERE Id = @Id", new { Id = id, Title = updatedSong.Title, Artist = updatedSong.Artist, Year = updatedSong.Year, DurationInSeconds = updatedSong.DurationInSeconds });
            }
        }

        public void DeleteSong(int id)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("DELETE FROM Songs WHERE Id = @Id", new { Id = id });
            }
        }
    }
}

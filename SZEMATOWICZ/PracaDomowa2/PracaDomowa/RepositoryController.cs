using PracaDomowa.MySQL;
using PracaDomowa.SQLITE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaDomowa
{
    internal class RepositoryController
    {
        public static MySQL.CarRepository GetMySQLCarRepository()
        {
            string mysqlConnectionString = "server=localhost;user=root;password=;database=praca_domowa;";
            return new MySQL.CarRepository(mysqlConnectionString);
        }

        public static MySQL.SongRepository GetMySQLSongRepository()
        {
            string mysqlConnectionString = "server=localhost;user=root;password=;database=praca_domowa;";
            return new MySQL.SongRepository(mysqlConnectionString);
        }


        public static SQLITE.CarRepository GetSQLITECarRepository()
        {
            string sqliteConnectionString = "Data Source=praca_domowa.db;Version=3;";
            return new SQLITE.CarRepository(sqliteConnectionString);
        }

        public static SQLITE.SongRepository GetSQLITESongRepository()
        {
            string sqliteConnectionString = "Data Source=praca_domowa.db;Version=3;";
            return new SQLITE.SongRepository(sqliteConnectionString);
        }
    }
}

using System.Data.SQLite;

namespace TRON_RayLib
{
    internal class HiScoreDBHelper
    {
        private readonly string connStr = "Data Source=tron_game.db;Version=3";

        public HiScoreDBHelper()
        {
            using var conn = new SQLiteConnection(connStr);
            conn.Open();
            string query = @"create table if not exists PlayerScores ("+
                            "match_id integer primary key autoincrement, "+
                            "p1_name text not null, " +
                            "p1_score integer not null, " +
                            "p2_name text not null, " +
                            "p2_score integer not null)";
            var cmd = new SQLiteCommand(query, conn);
            cmd.ExecuteNonQuery();
        }

        public void AddScore(string p1Name, int p1Score, string p2Name, int p2Score)
        {
            using var conn = new SQLiteConnection(connStr);
            conn.Open();
            string insert = "insert into PlayerScores (p1_name, p1_score, p2_name, p2_score) " +
                "values (@p1_name, @p1_score, @p2_name, @p2_score)";
            var cmd = new SQLiteCommand(insert, conn);
            cmd.Parameters.AddWithValue("@p1_name", p1Name);
            cmd.Parameters.AddWithValue("@p1_score", p1Score);
            cmd.Parameters.AddWithValue("@p2_name", p2Name);
            cmd.Parameters.AddWithValue("@p2_score", p2Score);
            cmd.ExecuteNonQuery();
        }

        public List<(string p1_name, int p1_score, string p2_name, int p2_score)> GetTopScores(int top = 10)
        {
            List<(string p1_name, int p1_score, string p2_name, int p2_score)> scores = [];

            using var conn = new SQLiteConnection(connStr);
            conn.Open();
            string select = "select p1_name, p1_score, p2_name, p2_score "+
                "from PlayerScores order by score desc limit @top";
            var cmd = new SQLiteCommand(select, conn);
            cmd.Parameters.AddWithValue("@top", top);
            SQLiteDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string p1Name = dr.GetString(0);
                int p1Score = dr.GetInt32(1);
                string p2Name = dr.GetString(2);
                int p2Score = dr.GetInt32(3);
                scores.Add((p1Name, p1Score, p2Name, p2Score));
            }
            
            return scores;
        }
    }
}

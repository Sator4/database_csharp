using System;
using Npgsql;
using System.Text.RegularExpressions;


namespace DataBaseApp {
    class Functions {

        public static void AddGuild(string name, int fund, int chief) {
            name = new Regex(@"\W").Replace(name, "");
            using (NpgsqlConnection con = GetConnection()) {
                if (!checkPlayerPresence(chief)) {
                    Console.WriteLine("No such player. To get the list of available players, print 'get_players'");
                }
                string query = @$"INSERT INTO Guilds (guild_id, guild_name, fund, chief, foundation_time)
                                    VALUES ({getLastGuildId() + 1}, '{name}', {fund}, {chief}, DATE(NOW()));";
                con.Open();
                var command = new NpgsqlCommand(query, con);
                try {
                    command.ExecuteNonQuery();
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
        }

        public static void AlterRelation(int guild_1, int guild_2, string relation) {
            var event_id = relation == "war" ? 9 : relation == "peace" ? 10 : relation == "alliance" ? 11 : -1;
            if (event_id == -1) {
                Console.WriteLine("Invalid relation");
                return;
            }
            using (NpgsqlConnection con = GetConnection()) {
                con.Open();
                var command = new NpgsqlCommand(@$"INSERT INTO Logs (e_time, player_id, player_2_id, guild_id, guild_2_id, event_id)
                                                VALUES (NOW(), NULL, NULL, {guild_1}, {guild_2}, {event_id})", con);
                try {
                    command.ExecuteNonQuery();
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }

        }

        public static string GetRelation(int guild_1, int guild_2) {
            using (NpgsqlConnection con = GetConnection()) {
                con.Open();
                var command = new NpgsqlCommand(@$"SELECT event_id FROM Logs WHERE guild_id = {guild_1} AND guild_2_id = {guild_2}
                                                    OR guild_2_id = {guild_1} AND guild_id = {guild_2}
                                                    ORDER BY e_time DESC LIMIT 1", con);
                try {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    if (!reader.HasRows || reader.GetInt32(0) == 10) {
                        return "peace";
                    }
                    else if (reader.GetInt32(0) == 9) {
                        return "war";
                    }
                    return "alliance";
                }
                catch (Exception e) {
                    return Convert.ToString(e);
                }
                
                
            }
        }

        public static void getPlayers() {
            NpgsqlConnection con = GetConnection();
            con.Open();
            string query = @"SELECT * FROM Players";
            var command = new NpgsqlCommand(query, con);
            try {
                NpgsqlDataReader reader = command.ExecuteReader();

                using (reader) {
                    Console.WriteLine("id, name, level, money, guild_id:");
                    while (reader.Read()) {
                        for (int i = 0; i < reader.FieldCount; i++) {
                            for (int j = 0; j < 5; j++) {
                                Console.Write(reader[i + j]);
                                Console.Write(", ");
                            }
                            i += 5;
                            Console.WriteLine();
                        }
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        public static void getGuilds() {
            NpgsqlConnection con = GetConnection();
            con.Open();
            string query = @"SELECT * FROM Guilds";
            var command = new NpgsqlCommand(query, con);
            try {
                NpgsqlDataReader reader = command.ExecuteReader();

                using (reader) {
                    Console.WriteLine("id, name, fund, chief, foundation_time, dismission_time:");
                    while (reader.Read()) {
                        for (int i = 0; i < reader.FieldCount; i++) {
                            for (int j = 0; j < 6; j++) {
                                Console.Write(reader[i + j]);
                                Console.Write(", ");
                            }
                            i += 6;
                            Console.WriteLine();
                        }
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        public static int getLastGuildId() {
            using (NpgsqlConnection con = GetConnection()) {
                con.Open();
                var command = new NpgsqlCommand(@"SELECT guild_id FROM Guilds ORDER BY guild_id DESC LIMIT 1", con);
                try {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    return reader.GetInt32(0);
                } catch (Exception e){
                    Console.WriteLine(e);
                    return -1;
                }
            }
        }

        public static int getLastPlayerId() {
            using (NpgsqlConnection con = GetConnection()) {
                con.Open();
                var command = new NpgsqlCommand(@"SELECT player_id FROM Players ORDER BY player_id DESC LIMIT 1", con);
                try {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    return reader.GetInt32(0);
                } catch (Exception e){
                    Console.WriteLine(e);
                    return -1;
                }
            }
        }

        public static bool checkPlayerPresence(int player_id) {
            using (NpgsqlConnection con = GetConnection()) {
                con.Open();
                var command = new NpgsqlCommand(@$"SELECT COUNT(*) FROM Players WHERE player_id = {player_id}", con);
                NpgsqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.GetInt32(0) > 0) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        private static NpgsqlConnection GetConnection() {
            return new NpgsqlConnection(@"Server=localhost; Port=5432; User Id = postgres; Password=root; DataBase=postgres");
        }
    }
}

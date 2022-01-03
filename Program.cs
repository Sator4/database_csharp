using System;


namespace DataBaseApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Build_db.Build();
            string input;
            while (true) {
                input = Console.ReadLine();
                string[] command = input.Split(' ');
                if (input == "exit" || input == "quit" || input == "q") {
                    break;
                }

                if (input == "help") {
                    Console.WriteLine("List of available commands:");
                    Console.WriteLine("add_guild [name] [fund] [chief]");
                    Console.WriteLine("get_players");
                    Console.WriteLine("get_guilds");
                    Console.WriteLine("get_relation [guild_1] [guild_2]");
                    Console.WriteLine("alter_relation [guild_1] [guild_2] [new_relation], where [new_relation] can be: war, peace, alliance");
                }
                else if (command[0] == "add_guild") {
                    bool i1 = true, i2 = true, r1 = true, r2 = true;
                    int guild_fund = -1, guild_chief = -1;

                    try {
                        guild_fund = Convert.ToInt32(command[2]);
                    } catch (FormatException) {
                        i1 = false;
                    } catch (IndexOutOfRangeException) {
                        r1 = false;
                    }
                    try {
                        guild_chief = Convert.ToInt32(command[3]);
                    } catch (FormatException) {
                        i2 = false;
                    } catch (IndexOutOfRangeException) {
                        r2 = false;
                    }
                    if (!i1) {
                        Console.WriteLine("Fund must be integer");}
                    if (!i2) {
                        Console.WriteLine("Chief id must be integer");}
                    if (!r1) {
                        Console.WriteLine("No fund information provided");}
                    if (!r2) {
                        Console.WriteLine("No chief information provided");}
                    if (command.Length < 2) {
                        Console.WriteLine("No name provided");
                    } else if (guild_chief == -1 || guild_fund == -1) {
                        Console.WriteLine("Unknown error");
                    } else if (i1 && i2 && r1 && r2) {
                        Functions.AddGuild(command[1], guild_fund, guild_chief);
                        Console.WriteLine("Success");
                    }
                }
                else if (command[0] == "get_relation") {
                    bool i = true, r1 = true, r2 = true;
                    int guild_1 = -1, guild_2 = -1;
                    try {
                        guild_1 = Convert.ToInt32(command[1]);
                    } catch (FormatException) {
                        i = false;
                    } catch (IndexOutOfRangeException) {
                        r1 = false;
                    }
                    try {
                        guild_2 = Convert.ToInt32(command[2]);
                    } catch (FormatException) {
                        i = false;
                    } catch (IndexOutOfRangeException) {
                        r2 = false;}
                    if (!i) {
                        Console.WriteLine("Guild id must be integer");}
                    if (!r1) {
                        Console.WriteLine("Guild 1 id is missing");}
                    if (!r2) {
                        Console.WriteLine("Guild 2 id is missing");}
                    if (guild_1 == -1 || guild_2 == -1) {
                        Console.WriteLine("Unknown error");
                    } else if (i && r1 && r2) {
                        Console.WriteLine(Functions.GetRelation(guild_1, guild_2));
                    }
                }
                else if (command[0] == "alter_relation") {
                    bool i = true, r1 = true, r2 = true;
                    int guild_1 = -1, guild_2 = -1;
                    try {
                        guild_1 = Convert.ToInt32(command[1]);
                    } catch (FormatException) {
                        i = false;
                    } catch (IndexOutOfRangeException) {
                        r1 = false;
                    }
                    try {
                        guild_2 = Convert.ToInt32(command[2]);
                    } catch (FormatException) {
                        i = false;
                    } catch (IndexOutOfRangeException) {
                        r2 = false;}
                    if (!i) {
                        Console.WriteLine("Guild id must be integer");}
                    if (!r1) {
                        Console.WriteLine("Guild 1 id is missing");}
                    if (!r2) {
                        Console.WriteLine("Guild 2 id is missing");}
                    if (guild_1 == -1 || guild_2 == -1) {
                        Console.WriteLine("Unknown error");
                    } else if (i && r1 && r2) {
                        Functions.AlterRelation(guild_1, guild_2, command[3]);
                    }
                }
                else if (command[0] == "get_players") {
                    Functions.getPlayers();

                }
                else if (command[0] == "get_guilds") {
                    Functions.getGuilds();
                }
                else {
                    Console.WriteLine("Invalid command");
                }
                Console.WriteLine();
            }
            
        }
    }
}

using System;
using Tobii.Interaction;

using Npgsql;
using System.Data;

namespace TobiiEyeTracker4CDataStream2
{
    class Program
    {
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();

        private const string Format = "{0}\t{1}\t{2}";

        static void Main(string[] args)
        {
            var connString = "Host=localhost;Username=postgres;Password=postgres;Database=easy";
            var host = new Host();
            var gazePointDataStream = host.Streams.CreateGazePointDataStream();

            // Open file to write data
            // FileStream fs = new FileStream(@"C:\Users\nando\Desktop\gaze_data.txt", FileMode.Append);
            // StreamWriter sw = new StreamWriter(fs);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                // 3. Get the gaze data!
                gazePointDataStream.GazePoint((x, y, ts) => {
                    string now = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    string point = string.Format(Format, (int)x, (int)y, now);
                    // sw.WriteLine(point);
                    Console.WriteLine(point);

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO experiments_point (x, y, datetime) VALUES (@x, @y, @datetime)";
                        cmd.Parameters.AddWithValue("x", "" + x);
                        cmd.Parameters.AddWithValue("y", "" + y);
                        cmd.Parameters.AddWithValue("datetime", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                });

                // okay, it is 4 lines, but you won't be able to see much without this one :)
                Console.ReadKey();

                // we will close the coonection to the Tobii Engine before exit.
                host.DisableConnection();
                conn.Close();
                // sw.Flush();
                // sw.Close();
                // fs.Close();
            }
        }
    }
}

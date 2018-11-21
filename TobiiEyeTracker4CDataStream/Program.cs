using System;
using System.IO;
using Tobii.Interaction;

namespace TobiiEyeTracker4CDataStream
{
    class Program
    {
        private const string Format = "{0}\t{1}\t{2}";

        static void Main(string[] args)
        {
            var host = new Host();

            var gazePointDataStream = host.Streams.CreateGazePointDataStream();
            Console.WriteLine("testanco");

            // Open file to write data
            FileStream fs = new FileStream(@"C:\Users\nando\Desktop\gaze_data.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);

            // 3. Get the gaze data!
            gazePointDataStream.GazePoint((x, y, ts) => {
                string now = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                string point = string.Format(Format, (int)x, (int)y, now);
                sw.WriteLine(point);
                Console.WriteLine(point);
            });

            // okay, it is 4 lines, but you won't be able to see much without this one :)
            Console.ReadKey();

            // we will close the coonection to the Tobii Engine before exit.
            host.DisableConnection();

            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}

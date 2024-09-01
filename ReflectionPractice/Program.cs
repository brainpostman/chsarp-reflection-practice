using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Text;

namespace ReflectionPractice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = 100000;
            Func<int, string> serCustom = n =>
            {
                F f = F.Get();
                string finalString = CustomCsv.Serialize(f);
                for (int i = 0; i < n - 1; i++)
                {
                    var str = CustomCsv.Serialize(f);
                }
                return finalString;
            };
            Func<int, string> deserCustom = n =>
            {
                F? f2;
                try
                {
                    using (FileStream fs = new FileStream("../../../test.csv", FileMode.Open))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        string textFromFile = Encoding.Default.GetString(buffer);
                        f2 = (F?)CustomCsv.Deserialize(typeof(F), textFromFile, ',');
                    }
                    for (int i = 0; i < n - 1; i++)
                    {
                        using (FileStream fs = new FileStream("../../../test.csv", FileMode.Open))
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            string textFromFile = Encoding.Default.GetString(buffer);
                            CustomCsv.Deserialize(typeof(F), textFromFile, ',');
                        }
                    }
                }
                catch
                {
                    return "ERROR";
                }
                return f2 == null ? "ERROR" : f2.ToString()!;
            };
            Func<int, string> serNsoft = n =>
            {
                F f = F.Get();
                string finalString = JsonConvert.SerializeObject(f);
                for (int i = 0; i < n - 1; i++)
                {
                    JsonConvert.SerializeObject(f);
                }
                return finalString;
            };
            Func<int, string> deserNsoft = n =>
            {
                F? f2;
                try
                {
                    using (FileStream fs = new FileStream("../../../test-json.json", FileMode.Open))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        string textFromFile = Encoding.Default.GetString(buffer);
                        f2 = JsonConvert.DeserializeObject<F>(textFromFile);
                    }
                    for (int i = 0; i < n - 1; i++)
                    {
                        using (FileStream fs = new FileStream("../../../test-json.json", FileMode.Open))
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            string textFromFile = Encoding.Default.GetString(buffer);
                            JsonConvert.DeserializeObject<F>(textFromFile);
                        }
                    }
                }
                catch
                {
                    return "ERROR";
                }
                return f2 == null ? "ERROR" : f2.ToString()!;
            };
            Test("Custom", n, serCustom, deserCustom);
            Test("NewtonSoft", n, serNsoft, deserNsoft);
        }

        static void Test(string lib, int iterations, Func<int, string> testSer, Func<int, string> testDeser)
        {
            Console.WriteLine($"---{lib}---");
            Console.WriteLine("Начинаем сериализацию n раз, n = {0}...", iterations);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            string finalString = testSer(iterations);
            watch.Stop();
            Console.WriteLine("Итог сериализации:");
            Console.WriteLine(finalString);
            Console.WriteLine($"Время выполнения: {watch.ElapsedMilliseconds} мс");
            Console.WriteLine("Начинаем десериализацию n раз, n = {0}...", iterations);
            watch.Start();
            finalString = testDeser(iterations);
            watch.Stop();
            Console.WriteLine("Итог десериализации:");
            Console.WriteLine(finalString);
            Console.WriteLine($"Время выполнения: {watch.ElapsedMilliseconds} мс");
            Console.WriteLine();
        }
    }
}


namespace HighwayControlPoint
{
    internal class Program
    {
        static Dictionary<int, string> SensorsToInitialize = new Dictionary<int, string>() {
            { 1, "km01" },
            { 2, "km02" },
            { 3, "km03" }
        };

        static void Main(string[] args)
        {
            var controlPoint = new ControlPoint(SensorsToInitialize);
            controlPoint.Start();
            Console.ReadLine();
        }
    }
}
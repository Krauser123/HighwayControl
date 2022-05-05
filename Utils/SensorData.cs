
namespace Utils
{
    public class SensorData
    {
        public double CarSpeed;
        public string CarPlate;
        public DateTime CatchDate;

        public SensorData(double carSpeed, string carPlate, DateTime catchDate)
        {
            this.CarSpeed = carSpeed;
            this.CarPlate = carPlate;
            this.CatchDate = catchDate;
        }
    }
}
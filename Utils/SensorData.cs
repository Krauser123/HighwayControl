
using System.Runtime.Serialization;

namespace Utils
{
    [DataContract]
    public class SensorData
    {
        [DataMember]
        public double CarSpeed { get; set; }
        
        [DataMember]
        public string CarPlate { get; set; }

        [DataMember]
        public DateTime CatchDate { get; set; }

        public SensorData()
        {

        }

        public SensorData(double carSpeed, string carPlate, DateTime catchDate)
        {
            this.CarSpeed = carSpeed;
            this.CarPlate = carPlate;
            this.CatchDate = catchDate;
        }
    }
}
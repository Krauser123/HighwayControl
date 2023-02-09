using System.Runtime.Serialization;

namespace Utils
{
    [DataContract]
    public class SensorCreateInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Km { get; set; }

        public SensorCreateInfo()
        {

        }
                
        public SensorCreateInfo(int id, string name, int km)
        {
            this.Id = id;
            this.Name = name;
            this.Km = km;
        }
    }
}
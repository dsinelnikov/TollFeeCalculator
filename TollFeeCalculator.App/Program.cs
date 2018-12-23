using System;
using System.IO;
using System.Xml.Serialization;
using TollFeeCalculator.App.Data;
using TollFeeCalculator.Core;
using TollFeeCalculator.Infrastructure.Configurations;
using TollFeeCalculator.Infrastructure.Vehicles;

namespace TollFeeCalculator.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var serializer = new XmlSerializer(typeof(TrafficData));
            var configuration = new SwedishCityConfiguration();
            var calculator = new Calculator(configuration);
            TrafficData trafficData;

            using (var fileStream = new FileStream("data.xml", FileMode.Open))
            {
                trafficData = (TrafficData)serializer.Deserialize(fileStream);
            }

            foreach (var item in trafficData.VehiclesTraffic)
            {
                var tollFee = calculator.CalculateTollFee(new Vehicle(item.Name, item.VehicleCode), item.Dates);

                Console.WriteLine($"{item.Name}:{item.VehicleCode}\t{tollFee}SEK");
            }

            Console.ReadLine();
        }
    }
}

using ParkDomain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParkDomain
{
    public class FeesFactory
    {
        Dictionary<int, Type> AvilableFees;
        public FeesFactory()
        {
            LoadFeesTypes();
        }

        public IParkFeesCalculator CreateInstance(int FactoryType)
        {
            foreach (var item in AvilableFees)
            {
                if(item.Key==FactoryType)
                {
                    //as vs direct casting
                    //https://stackoverflow.com/questions/132445/direct-casting-vs-as-operator
                    var temp = AvilableFees[item.Key];
                    return AvilableFees[item.Key] as IParkFeesCalculator;
                }
            }
            return null;
        }

        public void LoadFeesTypes()
        {
            AvilableFees = new Dictionary<int, Type>();
            Type[] typesInThisAssembly = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in typesInThisAssembly)
            {
                if (type.GetInterface(typeof(IParkFeesCalculator).ToString()) != null)
                {
                    var feesType = ((IParkFeesCalculator)Activator.CreateInstance(type)).FeesTypes;
                    // AvilableFees.Add(((IParkFeesCalculator)type).FeesTypes, type);
                    AvilableFees.Add(feesType, type);
                }
            }
        }
    }
}

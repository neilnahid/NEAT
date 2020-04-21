using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NEAT;
namespace NEAT.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static T PickRandomElement<T>(this IEnumerable<T> list)
        {
            if (list.Count() > 0)
            {
                return list.ToList()[Static.Random.Next(list.Count())];
            }
            return default(T);
        }
    }
}

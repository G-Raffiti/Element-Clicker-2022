using System;
using System.Collections.Generic;
using UnityEngine;

namespace BigNumbers
{
    // bn2 is a class to define production 
    [Serializable]
    public class bn2
    {
        // added are flat resource added to the production
        public bn Added;
        // multi are multiplied together (exemple: 0.75 (= -25%) 1.1 (= +10%) )
        public bn Multi;
        // increased will be added together and then multiplied with everything else
        // 1 = +100% // 10 = *10 // 1.2 = +120% // 0.3= +30%
        public bn Increased;
    
        // this is the result of the value below
        public bn Value => Added * Multi * Increased;

        // Constructor
        public bn2()
        {
            Added = new bn();
            Multi = 1;
            Increased = 0;
        }

        public bn2(bn added, bn multi, bn increased)
        {
            Added = new bn(added);
            Multi = new bn(multi);
            Increased = new bn(increased);
        }

        public bn2(bn2 a)
        {
            Added = new bn(a.Added);
            Multi = new bn(a.Multi);
            Increased = new bn(a.Increased);
        }

        public static implicit operator bn2(Vector3 a)
        {
            bn2 result = new bn2(a.x, a.y, a.z);
            return result;
        }
    }

    public static class Bn2Extension
    {
        // Operator
        public static void Merge(this bn2 a, bn2 b)
        {
            a.Added += b.Added;
            a.Multi *= b.Multi;
            a.Increased += b.Increased;
        }
    }
}
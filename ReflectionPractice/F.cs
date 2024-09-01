using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionPractice
{
    public class F 
    { 
        public int I1 { get; set; }
        public int I2 { get; set; }
        public int I3 { get; set; }    
        public int I4 { get; set; }    
        public int I5 { get; set; }

        public static F Get() => new F { I1 = 1, I2 = 2, I3 = 3, I4 = 4, I5 = 5 };
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DefaultNamespace
{
    public class Utils
    {
        public static IntPtr ConvertIntListToPtr(List<int> originalList)
        {
            int[] intArray = originalList.ToArray();
            
            IntPtr ptr = Marshal.AllocHGlobal(intArray.Length);
            Marshal.Copy(intArray, 0, ptr, intArray.Length);

            return ptr;
        }
        
        public static IntPtr ConvertDoubleListToPtr(List<double> originalList)
        {
            double[] doubleList = originalList.ToArray();
            
            IntPtr ptr = Marshal.AllocHGlobal(doubleList.Length);
            Marshal.Copy(doubleList, 0, ptr, doubleList.Length);

            return ptr;
        }
    }
}
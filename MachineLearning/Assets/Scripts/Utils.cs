using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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

    public static List<string> IntPtrToStringArrayAnsi(IntPtr ptr)
    {
        List<string> lst = new List<string>();

        do
        {
            lst.Add(Marshal.PtrToStringAnsi(ptr));

            while (Marshal.ReadByte(ptr) != 0)
            {
                ptr = IntPtr.Add(ptr, 1);
            }

            ptr = IntPtr.Add(ptr, 1);
        }
        while (Marshal.ReadByte(ptr) != 0);

        return lst;
    }
    
}
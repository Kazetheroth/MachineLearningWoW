// TestMLDLL.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>

extern "C"
{
    __declspec(dllimport) double my_add(double a, double b);
}

int main()
{
    std::cout << "Hello World!\n" << my_add(42, 51) << std::endl;
}

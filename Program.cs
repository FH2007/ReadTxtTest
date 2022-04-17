// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;
using TestApp;
Processor processor = new Processor();
ICheck check = new CheckStr();
string[] vs = 
    { 
    "D:/Work/TestTasks/repos/ProcessorCheckApp/ReadTxtTest/Data/Data1.txt",
    "D:/Work/TestTasks/repos/ProcessorCheckApp/ReadTxtTest/Data/Data1 Копировать.txt"
};
processor.Run(check, vs);
foreach (var line in processor.Dictionary)
{
    Console.WriteLine(line);
}
Console.ReadLine();


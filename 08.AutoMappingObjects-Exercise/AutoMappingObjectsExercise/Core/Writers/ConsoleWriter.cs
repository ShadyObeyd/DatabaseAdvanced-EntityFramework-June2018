namespace AutoMappingObjectsExercise.Core.Writers
{
    using Contracts;
    using System;

    public class ConsoleWriter : IWriter
    {
        public void WriteLine(string output)
        {
            Console.WriteLine(output);
        }
    }
}

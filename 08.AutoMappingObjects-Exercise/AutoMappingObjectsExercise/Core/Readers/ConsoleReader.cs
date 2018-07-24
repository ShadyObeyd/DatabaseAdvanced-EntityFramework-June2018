namespace AutoMappingObjectsExercise.Core.Readers
{
    using System;
    using Contracts;

    public class ConsoleReader : IReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}

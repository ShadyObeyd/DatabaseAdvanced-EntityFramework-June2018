namespace AutoMappingObjectsExercise.Core
{
    using Readers;
    using Contracts;
    using Data;
    using System;
    using Writers;

    public class Engine : IEngine
    {
        private ICommandInterpreter commandInterpreter;
        private IReader reader;
        private IWriter writer;

        public Engine(AutoMappingContext context)
        {
            this.commandInterpreter = new CommandInterpreter(context);
            this.reader = new ConsoleReader();
            this.writer = new ConsoleWriter();
        }

        public void Run()
        {
            string input = this.reader.ReadLine();

            while (input != "Exit")
            {
                string[] inputTokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    this.commandInterpreter.Read(inputTokens);
                }
                catch (ArgumentException ex)
                {
                    this.writer.WriteLine(ex.Message);
                }
                catch(InvalidOperationException ex)
                {
                    this.writer.WriteLine(ex.Message);
                }

                input = this.reader.ReadLine();
            }
        }
    }
}

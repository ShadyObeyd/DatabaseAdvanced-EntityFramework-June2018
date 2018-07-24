namespace AutoMappingObjectsExercise
{
    using Core;
    using Core.Contracts;
    using Data;

    public class StartUp
    {
        public static void Main()
        {
            using (AutoMappingContext context = new AutoMappingContext())
            {
                IEngine engine = new Engine(context);
                engine.Run();
            }
        }
    }
}
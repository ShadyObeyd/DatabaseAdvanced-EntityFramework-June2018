namespace AutoMappingObjectsExercise.Core.Commands
{
    using AutoMappingObjectsExercise.Core.Writers;
    using Contracts;

    public class ManagerInfoCommand : ICommand
    {
        private IManagerController managerController;
        private IWriter writer;

        public ManagerInfoCommand(IEmployeeController employeeController, IManagerController managerController)
        {
            this.managerController = managerController;
            this.writer = new ConsoleWriter();
        }

        public void Execute(string[] args)
        {
            var managerDTO = this.managerController.GetManagerInfo(args);

            this.writer.WriteLine(managerDTO.ToString());
        }
    }
}

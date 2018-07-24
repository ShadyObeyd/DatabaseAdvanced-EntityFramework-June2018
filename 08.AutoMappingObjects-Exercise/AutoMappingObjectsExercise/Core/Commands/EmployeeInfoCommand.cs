namespace AutoMappingObjectsExercise.Core.Commands
{
    using Writers;
    using Contracts;

    public class EmployeeInfoCommand : ICommand
    {
        private IEmployeeController employeeController;
        private IWriter writer;

        public EmployeeInfoCommand(IEmployeeController employeeController, IManagerController managerController)
        {
            this.employeeController = employeeController;
            this.writer = new ConsoleWriter();
        }

        public void Execute(string[] args)
        {
            var employeeDTO = this.employeeController.GetEmployeeInfo(args);

            this.writer.WriteLine(employeeDTO.ToString());
        }
    }
}

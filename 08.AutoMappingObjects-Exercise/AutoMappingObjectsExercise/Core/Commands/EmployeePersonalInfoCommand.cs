namespace AutoMappingObjectsExercise.Core.Commands
{
    using Writers;
    using Contracts;

    public class EmployeePersonalInfoCommand : ICommand
    {
        private IEmployeeController employeeController;
        private IWriter writer;

        public EmployeePersonalInfoCommand(IEmployeeController employeeController, IManagerController managerController)
        {
            this.employeeController = employeeController;
            this.writer = new ConsoleWriter();
        }

        public void Execute(string[] args)
        {
            var employeeFullInfoDTO = this.employeeController.GetEmployeePersonalInfo(args);

            this.writer.WriteLine(employeeFullInfoDTO.ToString());
        }
    }
}

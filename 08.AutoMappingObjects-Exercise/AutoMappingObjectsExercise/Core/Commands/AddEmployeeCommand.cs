namespace AutoMappingObjectsExercise.Core.Commands
{
    using Writers;
    using Contracts;

    public class AddEmployeeCommand : ICommand
    {
        private IEmployeeController employeeController;

        public AddEmployeeCommand(IEmployeeController employeeController, IManagerController managerController)
        {
            this.employeeController = employeeController;
        }

        public void Execute(string[] args)
        {
            this.employeeController.AddEmployee(args);
        }
    }
}

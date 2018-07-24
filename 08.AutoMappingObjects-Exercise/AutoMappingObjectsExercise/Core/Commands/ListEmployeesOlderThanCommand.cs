namespace AutoMappingObjectsExercise.Core.Commands
{
    using Contracts;
    using Writers;

    public class ListEmployeesOlderThanCommand : ICommand
    {
        IEmployeeController employeeController;
        IWriter writer;

        public ListEmployeesOlderThanCommand(IEmployeeController employeeController, IManagerController managerController)
        {
            this.employeeController = employeeController;
            this.writer = new ConsoleWriter();
        }
        public void Execute(string[] args)
        {
            var employees = this.employeeController.GetEmployeesOlderThan(args);

            foreach (var employee in employees)
            {
                this.writer.WriteLine(employee.ToString());
            }
        }
    }
}

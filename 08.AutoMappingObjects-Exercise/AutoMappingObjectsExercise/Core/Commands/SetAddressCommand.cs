namespace AutoMappingObjectsExercise.Core.Commands
{
    using Contracts;
    using Writers;

    public class SetAddressCommand : ICommand
    {
        private IEmployeeController employeeController;

        public SetAddressCommand(IEmployeeController employeeController, IManagerController managerController)
        {
            this.employeeController = employeeController;
        }

        public void Execute(string[] args)
        {
            this.employeeController.SetAddress(args);
        }
    }
}

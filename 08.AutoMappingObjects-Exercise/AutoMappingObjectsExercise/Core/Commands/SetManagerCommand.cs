namespace AutoMappingObjectsExercise.Core.Commands
{
    using Contracts;

    public class SetManagerCommand : ICommand
    {
        private IManagerController managerController;

        public SetManagerCommand(IEmployeeController employeeController, IManagerController managerController)
        {
            this.managerController = managerController;

        }
        public void Execute(string[] args)
        {
            this.managerController.SetManager(args);
        }
    }
}

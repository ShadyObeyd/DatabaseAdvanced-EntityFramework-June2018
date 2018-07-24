namespace AutoMappingObjectsExercise.Core.Commands
{
    using Contracts;
    using Writers;

    public class SetBirthdayCommand : ICommand
    {
        private IEmployeeController employeeController;

        public SetBirthdayCommand(IEmployeeController employeeController, IManagerController managerController)
        {
            this.employeeController = employeeController;
        }

        public void Execute(string[] args)
        {
            this.employeeController.SetBirthday(args);
        }
    }
}

namespace AutoMappingObjectsExercise.Core.Contracts
{
    using Data.Models.DTOs;

    public interface IManagerController
    {
        void SetManager(string[] args);

        ManagerDTO GetManagerInfo(string[] args);
    }
}

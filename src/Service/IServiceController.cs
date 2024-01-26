using dotnet_project.src.DAO;

namespace dotnet_project.src.Service;

public interface IServiceController
{
    static abstract void AddOperation(double amount, Operation.Categories categories, bool income);
    static abstract List<Operation> GetOperationsFor(string path, DateTime dateTime);
}
using dotnet_project.DAO;

namespace dotnet_project.Service;

public interface IServiceController
{
    void AddNewOperation(string path ,double amount, Operation.Categories categories, bool income);
    void AddNewOperation(string path ,double amount, bool income);
    bool RemoveOperation(string path, string id);
    void EditOperation(string path, string id, Operation newOperation);
    List<Operation> FetchOperationsFor(string path, int year, int month);
    double GetBalance(string path);
    void SetBalance(string path, double value);
}
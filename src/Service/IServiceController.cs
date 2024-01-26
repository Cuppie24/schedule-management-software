using dotnet_project.src.DAO;

namespace dotnet_project.src.Service;

public interface IServiceController
{
    void AddOperation(double amount, Operation.Categories categories, bool income);
    void GetStatisticsFor(DateTime dateTime);
}
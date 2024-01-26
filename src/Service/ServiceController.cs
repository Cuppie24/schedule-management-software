using dotnet_project.src.DAO;
using dotnet_project.src.DTO;

namespace dotnet_project.src.Service;

public class ServiceController : IServiceController
{
    private const string OperationsFilePath = @"D:\Projects\schedule-management-software\Source\Recourses\Operations.csv";

    public void AddOperation(double amount, Operation.Categories categories, bool income)
    {
        string id = GenerateId();
        var dummy = new Operation()
        {
            Id = id,
            Amount = amount,
            Income = income,
            DateTime = DateTime.Now,
            Category = categories
        };
        DtoController.Add(OperationsFilePath, dummy);
    }

    public void GetStatisticsFor(DateTime dateTime)
    {
    }

    private string GenerateId()
    {
        var guid = Guid.NewGuid();
        return guid.ToString("N");
    }
}
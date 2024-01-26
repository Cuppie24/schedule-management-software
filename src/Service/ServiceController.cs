using dotnet_project.src.DAO;
using dotnet_project.src.DTO;

namespace dotnet_project.src.Service;

public class ServiceController : IServiceController
{
    private const string OperationsFilePath = @"D:\Projects\schedule-management-software\Source\Recourses\Operations.csv";

    public static void AddOperation(double amount, Operation.Categories categories, bool income)
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

    public static List<Operation> GetOperationsFor(string path, DateTime dateTime)
    {
        var resultList = new List<Operation>();
        List<Operation> operations = DtoController.FetchAll(path);
        foreach (var variable in operations)
        {
            if(variable.DateTime.Year.Equals(dateTime.Year) && variable.DateTime.Month.Equals(dateTime.Month))
                resultList.Add(variable);
        }
        return resultList;
    }

    private static string GenerateId()
    {
        var guid = Guid.NewGuid();
        return guid.ToString("N");
    }
}
using dotnet_project.DAO;
using dotnet_project.DTO;

namespace dotnet_project.Service;

public class ServiceController : IServiceController
{
    private IDtoController<Operation> _operationsDtoController = new DtoController();
    public void AddNewOperation(string path ,double amount, Operation.Categories category, bool income)
    {
        var id = GenerateId();
        var dummy = new Operation()
        {
            Id = id,
            Amount = amount,
            Income = income,
            DateTime = DateTime.Now,
            Category = category
        };
        _operationsDtoController.Add(path, dummy);
    }
    
    public void AddNewOperation(string path ,double amount, bool income)
    {
        var id = GenerateId();
        var dummy = new Operation()
        {
            Id = id,
            Amount = amount,
            Income = income,
            DateTime = DateTime.Now,
            Category = Operation.Categories.Undefined
        };
        _operationsDtoController.Add(path, dummy);
    }
    
    public bool RemoveOperation(string path, string id) => _operationsDtoController.Remove(path, id);
    public void EditOperation(string path, string id, Operation operation) => _operationsDtoController.Edit(path, id, operation);

    public double GetBalance(string path) => _operationsDtoController.GetBalance(path);
    public void SetBalance(string path, double value) => _operationsDtoController.SetBalance(path, value);
    
    public List<Operation> FetchOperationsFor(string path, int year, int month)
    {
        var operations = _operationsDtoController.FetchAll(path);
        var resultList = FilterByYearAndMonth(operations, year, month);
        return resultList;
    }
    
    private  List<Operation> FilterByYearAndMonth(List<Operation> operations, int year, int month)
    {
        var resultList = new List<Operation>();
        foreach (var variable in operations)
        {
            if(variable.DateTime.Year.Equals(year) && variable.DateTime.Month.Equals(month))
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
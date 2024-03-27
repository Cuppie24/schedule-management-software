using dotnet_project.DAO;

namespace dotnet_project.DTO;

public interface IDtoController<T>
{
    List<T> FetchAll(string path);
    List<T> FetchFiltered(string path, int index, string value);
    List<T> FetchFiltered(List<T> listToFilter, int index, string value);
    void Add(string path, Operation operationToAdd);
    bool Remove(string path, string index);
    void Edit(string path, string id, Operation newOperation);
    double GetBalance(string path);
    void SetBalance(string path, double value);
}

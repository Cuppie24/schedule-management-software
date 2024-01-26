using dotnet_project.src.DAO;

namespace dotnet_project.src.DTO;

public interface IDtoController <T>
{
    static abstract List<T> FetchAll(string path);
    static abstract List<T> FetchFiltered(string path, int index, string value);
    static abstract List<T> FetchFiltered(List<T> listToFilter, int index, string value);
    static abstract void Add(string path, Operation operationToAdd);
    static abstract void PrintAll(List<T> listToPrint);
}
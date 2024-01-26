using System.Globalization;
using System.Text;
using dotnet_project.src.DAO;

namespace dotnet_project.src.DTO;

public abstract class DtoController : IDtoController<Operation>
{
    private const string Delimiter = ",";
    
    public static List<Operation> FetchAll(string path)
    {
        var stringArrayList = new List<string[]>();
        using var streamReader = new StreamReader(path);
        while (streamReader.ReadLine() is { } line)
        {
            var array = line.Split(Delimiter);
            stringArrayList.Add(array);
        }
        // remove header
        stringArrayList.RemoveAt(0);
        
        return GetDaoFromStringList(stringArrayList);
    }

    public static List<Operation> FetchFiltered(string path, int index, string value)
    {
        var stringArrayList = new List<string[]>();
        using var streamReader = new StreamReader(path);
        while (streamReader.ReadLine() is { } line)
        {
            string[] array = line.Split(Delimiter);
            if (array[index] == value) stringArrayList.Add(array);
        }
        //remove header
        stringArrayList.RemoveAt(0);
        
        return GetDaoFromStringList(stringArrayList);
    }

    public static List<Operation> FetchFiltered(List<Operation> listToFilter, int index, string value)
    {
        var stringArrayList = new List<string[]>();
        foreach (var operation in listToFilter)
        {
            string[] array = GetStringFromDao(operation).Split(Delimiter);
            if (array[index].Equals(value)) stringArrayList.Add(array);
        }
        return GetDaoFromStringList(stringArrayList);
    }

    public static void Add(string path, Operation operationToAdd)
    {
        // check if the last line is empty 
        var isLastCharacterIsANewLineCharacter = false;
        using (var fileStream = new FileStream(path, FileMode.Open))
        {
            fileStream.Seek(-1, SeekOrigin.End);
            var lastChar = fileStream.ReadByte();
            if (lastChar is '\n') isLastCharacterIsANewLineCharacter = true;
        }
        // write to file
        using var streamWriter = new StreamWriter(path, true);
        if(!isLastCharacterIsANewLineCharacter) streamWriter.WriteLine();
        streamWriter.WriteLine(GetStringFromDao(operationToAdd));
    }

    public static void PrintAll(List<Operation> listToPrint)
    {
        foreach (Operation user in listToPrint)
        { 
            user.Print();
        }
    }

    private static List<Operation> GetDaoFromStringList(List<string[]> list)
    {
        List<Operation> resultDaoList = new List<Operation>();
        foreach (var array in list)
        {
            var dummy = new Operation() { Id = array[0] };
            if (double.TryParse(array[1], out var amount)) dummy.Amount = amount;
            if (bool.TryParse(array[2], out var income)) dummy.Income = income;
            if (DateTime.TryParse(array[3], out var dateTime)) dummy.DateTime = dateTime;
            if (Enum.TryParse(array[4], out Operation.Categories category)) dummy.Category = category;
            resultDaoList.Add(dummy);
        }
        return resultDaoList;
    }

    private static string GetStringFromDao(Operation operation)
    {
        var line = new StringBuilder();
        line.Append(operation.Id + ","); //id
        line.Append(operation.Amount.ToString(CultureInfo.InvariantCulture) + ","); //amount
        line.Append(Convert.ToInt32(operation.Income).ToString() + ","); //income
        line.Append(operation.DateTime.ToString(CultureInfo.InvariantCulture) + ","); //date time
        line.Append(operation.Category.ToString()); // category
        return line.ToString();
    }
}
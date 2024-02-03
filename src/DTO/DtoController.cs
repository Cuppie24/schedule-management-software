using System.Globalization;
using System.Text;
using System.Transactions;
using dotnet_project.DAO;

namespace dotnet_project.DTO;

public class DtoController : IDtoController<Operation>
{
    private const string Delimiter = ",";
    private const int StartingLineNumber = 3;
    
    public  List<Operation> FetchAll(string path)
    {
        var stringArrayList = new List<string[]>();
        using var streamReader = new StreamReader(path);
        while (streamReader.ReadLine() is { } line)
        {
            var array = line.Split(Delimiter); // надо добавить проверку на длину массива
            stringArrayList.Add(array);
        }

        // ignoring unnecessary leading lines
        for (var i = 0; i < StartingLineNumber; i++)
        {
            stringArrayList.RemoveAt(i);
        }
        return GetDaoFromStringList(stringArrayList);
    }

    public  List<Operation> FetchFiltered(string path, int index, string value)
    {
        var stringArrayList = new List<string[]>();
        using var streamReader = new StreamReader(path);
        while (streamReader.ReadLine() is { } line)
        {
            string[] array = line.Split(Delimiter); // надо добавить проверку на длину массива
            if (array[index] == value) stringArrayList.Add(array);
        }

        // ignoring unnecessary leading lines
        for (var i = 0; i < StartingLineNumber; i++)
        {
            stringArrayList.RemoveAt(i);
        }
        return GetDaoFromStringList(stringArrayList);
    }
    
    public  List<Operation> FetchFiltered(List<Operation> listToFilter, int index, string value)
    {
        var stringArrayList = new List<string[]>();
        foreach (var operation in listToFilter)
        {
            string[] array = GetStringFromDao(operation).Split(Delimiter); // надо добавить проверку на длину массива
            if (array[index].Equals(value)) stringArrayList.Add(array);
        }

        return GetDaoFromStringList(stringArrayList);
    }

    public  void Add(string path, Operation operationToAdd)
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
        using (var streamWriter = new StreamWriter(path, true))
        {
            if (!isLastCharacterIsANewLineCharacter)
                streamWriter.WriteLine(); // переход на новую строку если указатель стоит не на пустой строке
            streamWriter.WriteLine(GetStringFromDao(operationToAdd));
        }
        
        //set new balance
        var balance = GetBalance(path);
        if (operationToAdd.Income) balance += operationToAdd.Amount;
        else balance -= operationToAdd.Amount;
        SetBalance(path, balance);
    }

    public  bool Remove(string path, string id)
    {
        // indexes of values in csv file
        int amountIndex = 1, incomeIndex = 2;
        List<string[]> stringList = [];
        // read csv file and remove line with id
        using (var streamReader = new StreamReader(path))
        {
            while (streamReader.ReadLine() is { } line)
            {
                stringList.Add(line.Split(Delimiter));
            }

            var operationExists = false; // for return
            for (var i = 0; i < stringList.Count; i++)
            {
                if (stringList.ElementAt(i)[0].Equals(id))
                {
                    // update balance
                    double balance = GetBalance(path);
                    if (double.TryParse(stringList.ElementAt(i)[amountIndex], out var amount))
                    {
                        SetBalance(path,
                            stringList.ElementAt(i)[incomeIndex].Equals("0") ? balance + amount : balance - amount);
                    }
                    else throw new InvalidOperationException("Failed to parse amount from file");
                    //remove operation
                    stringList.RemoveAt(i);
                    operationExists = true;
                }
            }

            return operationExists;
        }
        // rewrite file without deleted id
        using (var streamWriter = new StreamWriter(path))
        {
            foreach (var separatedLine in stringList)
            {
                string line = "";
                // build line from string array with separator ";"
                for (int i = 0; i < separatedLine.Length; i++)
                {
                    line += separatedLine[i];
                    if (i != separatedLine.Length - 1) line += Delimiter;
                }
                
                streamWriter.WriteLine(line);
            }
        }
        return true;
    }

    public  double GetBalance(string path)
    {
        using var streamReader = new StreamReader(path);
        if (double.TryParse(streamReader.ReadLine(), out var parsedBalance)) 
            return parsedBalance;
            
        throw new InvalidOperationException("Failed to parse balance from file");
    }

    public  void SetBalance(string path ,double value)
    {
        List<string> lines = [];
        using (var streamReader = new StreamReader(path))
        {
            while (streamReader.ReadLine() is { } line)
            {
                lines.Add(line);
            }
        }
        lines[0] = value.ToString(CultureInfo.InvariantCulture);
        using var streamWriter = new StreamWriter(path);
        foreach (var line in lines)
        {
            streamWriter.WriteLine(line);
        }
    }

    public  void PrintList(List<Operation> listToPrint)
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
            if (DateTime.TryParse(array[3],CultureInfo.InvariantCulture,  out var dateTime)) dummy.DateTime = dateTime;
            if (Enum.TryParse(array[4], out Operation.Categories category)) dummy.Category = category;
            resultDaoList.Add(dummy);
        }

        return resultDaoList;
    }

    private static string GetStringFromDao(Operation operation) // сделать так чтобы он возвращал массив строк разделенный delimiter
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
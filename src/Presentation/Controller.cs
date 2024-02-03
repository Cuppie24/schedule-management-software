using dotnet_project.DAO;
using dotnet_project.Service;

namespace dotnet_project.Presentation;
public class Controller
{
    IServiceController _serviceController = new ServiceController();
    private const string FilePath = @"D:\Projects\schedule-management-software\src\Recourses\Operations.csv";
    private int _currentYear, _currentMonth;
    public void StartController()
    {
        _currentYear = DateTime.Now.Year;
        _currentMonth = DateTime.Now.Month;
        ShowMainMenu();
    }
    private void ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            PrintStatistics(); 
            Console.WriteLine("1 - new purchase\r\n" +
                              "2 - top-up\r\n" +
                              "3 - show operations history\r\n" +
                              "4 - set balance manually\r\n" +
                              "5 - edit operation" +
                              "6 - exit program");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    NewPurchase();
                    break;
                case "2":
                    TopUp();
                    break;
                case "3":
                    PrintOperationsForMonth();
                    break;
                case "4":
                    SetBalance();
                    break;
                case "5":
                    EditOperation();;
                    return;
                case "6":
                    return;
                default:
                    Console.WriteLine("Incorrect input!");
                    break;
            }
        }
    }
    private void PrintStatistics()
    {
        // get statistics from Operations list
        List<Operation> operations = _serviceController.FetchOperationsFor(FilePath, _currentYear, _currentMonth);
        double sumOfCategoryFamily = 0,
            sumOfCategoryStudy = 0,
            sumOfCategoryHealth = 0,
            sumOfCategoryFood = 0,
            sumOfCategoryCloth = 0,
            sumOfCategoryEntertainment = 0;
        foreach (var variable in operations)
        {
            switch (variable.Category)
            {
                case Operation.Categories.Family:
                    sumOfCategoryFamily += variable.Amount;
                    break;
                case Operation.Categories.Study:
                    sumOfCategoryStudy += variable.Amount;
                    break;
                case Operation.Categories.Health:
                    sumOfCategoryHealth += variable.Amount;
                    break;
                case Operation.Categories.Food:
                    sumOfCategoryFood += variable.Amount;
                    break;
                case Operation.Categories.Cloth:
                    sumOfCategoryCloth += variable.Amount;
                    break;
                case Operation.Categories.Entertainment:
                    sumOfCategoryEntertainment += variable.Amount;
                    break;
                default: throw new Exception("Incorrect category");
            }
        }
        // print statistics
        Console.WriteLine($"Statistics for month: {_currentYear}.{_currentMonth}\r\n" +
                          $"Your balance: {_serviceController.GetBalance(FilePath)}\r\n" +
                          $"Family: {sumOfCategoryFamily}\r\n" +
                          $"Study: {sumOfCategoryStudy}\r\n" +
                          $"Health: {sumOfCategoryHealth}\r\n" +
                          $"Food: {sumOfCategoryFood}\r\n" +
                          $"Cloth: {sumOfCategoryCloth}\r\n" +
                          $"Entertainment: {sumOfCategoryEntertainment}\r\n");
    }
    private void NewPurchase()
    {
        while (true)
        {
            Console.Clear();
            double amount;
            Operation.Categories category;
            Console.WriteLine("Enter amount: ");
            if (double.TryParse(Console.ReadLine(), out var parsedAmount))
                amount = parsedAmount;
            else continue;
            Console.WriteLine("Choose category: ");
            foreach (var variable in Enum.GetNames(typeof(Operation.Categories)).Select((name, index) => $"{index + 1} - {name}"))
            {
                Console.WriteLine(variable);
            }

            // subtract 1 from the input index
            int categoryIndex = int.TryParse(Console.ReadLine(), out var parsedIndex) ? parsedIndex - 1 : -1;

            Operation.Categories[] categoriesArray = Enum.GetValues<Operation.Categories>();
            if (categoryIndex >= 0 && categoryIndex <= categoriesArray.Length - 1)
                category = categoriesArray[categoryIndex];
            else
            {
                Console.WriteLine("incorrect category!\r\npress any key to continue");
                Console.ReadKey();
                continue;
            }

            _serviceController.AddNewOperation(FilePath, amount, category, false);
            break;
        }
    }

    private void TopUp()
    {
        while (true)
        {
            Console.WriteLine("Enter amount: ");
            if (double.TryParse(Console.ReadLine(), out var amount))
            {
                _serviceController.AddNewOperation(FilePath, amount, true);
                return;
            }
            Console.WriteLine("incorrect input");
        }
    }
     
    private void EditOperation()
    {
        PrintOperationsForMonth();
        Console.WriteLine("\r\nEnter the index of operation you want to edit ");
        if (int.TryParse(Console.ReadLine(), out var index))
        {
            
        }
    }

    private void PrintOperationsForMonth()
    {
        Console.Clear();
        Console.WriteLine($"Operations history for {_currentYear}.{_currentMonth}");
        List<Operation> operations =
            _serviceController.FetchOperationsFor(FilePath, _currentYear, _currentMonth);
        if (operations.Count != 0)
        {
            var counter = 1;
            foreach (var variable in operations)
            {
                Console.Write($"{counter}. ");
                variable.Print();
                counter++;
            }

            Console.WriteLine("\r\nPress any key to return to previous page");
            Console.ReadKey();
        }
        else Console.WriteLine("No operations for this month");
    }

    private void SetBalance()
    {
        Console.WriteLine("Enter your current balance: ");
        if (double.TryParse(Console.ReadLine(), out var newBalance))
        {
            _serviceController.SetBalance(FilePath ,newBalance);
        }
    }
    private void ChangeMonth()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Enter year: ");
            if (int.TryParse(Console.ReadLine(), out var parsedYear))
            {
                _currentYear = parsedYear;
            }
            else
            {
                Console.WriteLine("Incorrect year");
                continue;
            }

            Console.WriteLine("Enter month: ");
            if (int.TryParse(Console.ReadLine(), out var parsedMonth))
            {
                _currentMonth = parsedMonth;
                break;
            }
            Console.WriteLine("Incorrect month");
        }
    }
}
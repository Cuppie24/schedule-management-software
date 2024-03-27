using dotnet_project.DAO;
using dotnet_project.Service;

namespace dotnet_project.Presentation;
public class Controller
{
    IServiceController _serviceController = new ServiceController();
    private const string FilePath = @"D:\Projects\schedule-management-software\src\Recourses\Operations.csv";
    private int _currentYear = DateTime.Now.Year, _currentMonth = DateTime.Now.Month;
    private readonly Dictionary<string, Action> _menuItems = new();

    public Controller()
    {
        _menuItems.Add("1", NewPurchase);
        _menuItems.Add("2", TopUp);
        _menuItems.Add("3", PrintOperationsForCurrentMonth);
        _menuItems.Add("4", SetBalance);
        _menuItems.Add("5", EditOperation);
        _menuItems.Add("6", ChangeMonth);
    }

    public void StartController()
    {
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            PrintStatistics();
            DisplayMenu();
            Console.WriteLine("X - exit program");
            
            var input = Console.ReadLine();
            if (input is null)
            {
                Console.WriteLine("Incorrect input");
                continue;
            }
            if (_menuItems.ContainsKey(input))
            {
                _menuItems[input]();
            }
            else if (input.ToLower() is "x")
            {
                break;
            }
            else
            {
                Console.WriteLine("Incorrect input!");
            }
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine("1 - new purchase");
        Console.WriteLine("2 - top-up");
        Console.WriteLine("3 - show operations history");
        Console.WriteLine("4 - set balance manually");
        Console.WriteLine("5 - edit operation");
        Console.WriteLine("6 - Change month");
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
        List<Operation> operations = _serviceController.FetchOperationsFor(FilePath, _currentYear, _currentMonth);
        if (operations.Count == 0)
        {
            Console.WriteLine("There is no operations for this month");
            Console.ReadKey();
            return;
        }
        while (true)
        {
            PrintOperationsForCurrentMonth();
            Console.WriteLine("\nEnter the index of operation you want to edit: ");
            if (Int32.TryParse(Console.ReadLine(), out var indexOfEditingOperation))
            {
                Console.WriteLine(operations[indexOfEditingOperation - 1].ToString());
                Console.WriteLine("What parameter do you want to change ?\r\n" +
                                  "1 - amount\n" +
                                  "2 - category\n" +
                                  "3 - delete operation");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.WriteLine("Enter the new value: ");
                        if (double.TryParse(Console.ReadLine(), out var amount))
                        {
                            _serviceController.EditOperation(FilePath, operations[indexOfEditingOperation - 1].Id, 
                                operations[indexOfEditingOperation - 1] with {Amount =  amount} );
                            return;
                        }
                        break;
                    case "2":
                        Console.WriteLine("Choose category: ");
                        foreach (var variable in Enum.GetNames(typeof(Operation.Categories)).Select((name, index) => $"{index + 1} - {name}"))
                        {
                            Console.WriteLine(variable);
                        }
                        int categoryIndex = int.TryParse(Console.ReadLine(), out var parsedIndex) ? parsedIndex - 1 : -1;
                        Operation.Categories category;

                        Operation.Categories[] categoriesArray = Enum.GetValues<Operation.Categories>();
                        if (categoryIndex >= 0 && categoryIndex <= categoriesArray.Length - 1)
                            category = categoriesArray[categoryIndex];
                        else
                        {
                            Console.WriteLine("incorrect category!\r\npress any key to continue");
                            Console.ReadKey();
                            continue;
                        }
                        _serviceController.EditOperation(FilePath, operations[indexOfEditingOperation - 1].Id, operations[indexOfEditingOperation - 1] with {Category = category} );
                        return;
                    case "3":
                        _serviceController.RemoveOperation(FilePath, operations[indexOfEditingOperation - 1].Id);
                        return;
                    default:
                        Console.WriteLine("Incorrect input");
                        continue;
                }
            }
            else
            {
                Console.WriteLine("No operation with this index was found");
            }
        }
    }

    private void PrintOperationsForCurrentMonth()
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
                Console.WriteLine(variable.ToString());
                counter++;
            }
        }
        else Console.WriteLine("No operations for this month");
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
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
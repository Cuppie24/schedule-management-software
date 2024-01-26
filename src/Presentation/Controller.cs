using System.Text;
using dotnet_project.src.DAO;
using dotnet_project.src.Service;

namespace dotnet_project.src.Presentation;

public class Controller
{
    private string _filePath = "D:\\Projects\\schedule-management-software\\src\\Recourses\\Operations.csv";
    private DateTime _currentDate;
    public void StartController()
    {
        _currentDate = DateTime.Now;
        PrintStartMenu();
    }

    private void PrintStartMenu()
    {
        var menu = new StringBuilder();
        List<Operation> operations = ServiceController.GetOperationsFor(_filePath, _currentDate);
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
        
        menu.Append($"Statistics for month {_currentDate.Year}.{_currentDate.Month}\r\n");
        menu.Append($"Your balance: NOT COMPLETED\r\n");
        menu.Append($"Family: {sumOfCategoryFamily}\r\n");
        menu.Append($"Study: {sumOfCategoryStudy}\r\n");
        menu.Append($"Health: {sumOfCategoryHealth}\r\n");
        menu.Append($"Food: {sumOfCategoryFood}\r\n");
        menu.Append($"Cloth: {sumOfCategoryCloth}\r\n");
        menu.Append($"Entertainment: {sumOfCategoryEntertainment}\r\n");
        Console.WriteLine(menu);
    }
}
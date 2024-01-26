using System.Text;

namespace dotnet_project.src.Presentation;

public class Controller
{
    private DateTime _currentDate;
    public void StartController()
    {
        _currentDate = DateTime.Now;
        PrintStartMenu();
    }

    private void PrintStartMenu()
    {
        var menu = new StringBuilder();
        menu.Append($"Month: {_currentDate.Month} {_currentDate.Year}");
        
        Console.WriteLine(menu);
    }
}
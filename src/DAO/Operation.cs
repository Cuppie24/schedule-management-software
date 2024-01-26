using System.Text;

namespace dotnet_project.src.DAO;

public class Operation
{
    private string? _id;
    private double _amount;
    private bool _income;
    private DateTime _dateTime;
    private Categories _category = Categories.Indefined;

    public Operation(string id, double amount, bool income, DateTime dateTime, Categories categories)
    {
        Id = id;
        Category = categories;
        Amount = amount;
        Income = income;
        DateTime = dateTime;
    }
    public Operation()
    {
    }

    public string? Id
    {
        get => _id;
        set => _id = value;
    }
    public double Amount
    {
        get => _amount;
        set => _amount = value;
    }
    public bool Income
    {
        get => _income;
        set => _income = value;
    }
    public DateTime DateTime
    {
        get => _dateTime;
        set => _dateTime = value;
    }
    public Categories Category
    {
        get => _category;
        set => _category = value;
    }
    
    public enum Categories
    {
        Family,
        Study,
        Health,
        Food,
        Cloth,
        Entertainment,
        Indefined
    }
    public void Print()
    {
        var output = new StringBuilder();
        output.Append($"Id: {Id}; ");
        output.Append($"Category: {Category}; ");
        output.Append($"Amount: {Amount}; ");
        output.Append($"Income: {Income}; ");
        output.Append($"Date time: {DateTime}; ");
    }
}
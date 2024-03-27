namespace dotnet_project.DAO;

public record class Operation
{
    public string Id { get; init; }
    public double Amount { get; set; }
    public bool Income { get; set; }
    public DateTime DateTime { get; set; }
    public Categories Category { get; set; } = Categories.Undefined;

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
    
    public enum Categories
    {
        Family,
        Study,
        Health,
        Food,
        Cloth,
        Entertainment,
        Undefined
    }
    public override string ToString()
    {
        string output = Income ? "Top-up - " : "Purchase: - ";
        output += ($"Amount: {Amount} Date time: {DateTime} ");
        if(!Income) output += ($"Category: {Category} ");
        return output;
    }
}
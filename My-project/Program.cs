namespace Study
{
	class Program
	{
		public static void Main(string[] args)
		{
			Person person = new Person("name");
			Console.ReadKey();
        }
	}
	class Person
	{
		public string Name;
		public Person(string Name)
		{
			this.Name = Name;
		}
	}
	class Employee : Person
	{
		public string Company;
		public Employee(string Name, string Company) : base(Name)
		{
			this.Company = Company;
		}
	}
}
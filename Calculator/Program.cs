using Npgsql;

string[] operations = { "+", "-", "*", "/", "q" };
var calculator = new Calculator.Calculator();
string operation = "";


while (operation != "q")
{
    GenerateUi();
    if (operation != "q")
    {
        Operate();
    }
}

void Operate()
{
    if (operations.Contains(operation))
    {
        double first = PromptForNumber("Give me the first number:");
        double second = PromptForNumber("Give me the second number:");
        double result = Calculate(operation, first, second);
        Console.WriteLine($"Result: {result}");
        SaveRecord(operation, first, second, result);
    }
    else
    {
        Console.WriteLine("Invalid input.");
    }
}

void GenerateUi(){
    Console.WriteLine("Give me an operation (+,-,*,/, q - quit): ");
    operation = Console.ReadLine() ?? "q";
}
double PromptForNumber(string message)
{
    Console.WriteLine(message);
    return double.TryParse(Console.ReadLine(), out double number) ? number : 0;
}

double Calculate(string calcOperation, double a, double b)
{
    return calcOperation switch
    {
        "+" => calculator.Add(a, b),
        "-" => calculator.Subtract(a, b),
        "*" => calculator.Multiply(a, b),
        "/" when b != 0 => calculator.Divide(a, b),
        "/" => double.NaN, // Handle division by zero
        _ => 0,
    };
}

void SaveRecord(string operation, double first, double second, double result)
{
    var record = new Record
    {
        First = first,
        Second = second,
        Operator = operation,
        Result = result
    };

    
    var uriString = Environment.GetEnvironmentVariable("calcConnStr");

    if (uriString is null)
    {
        Console.WriteLine("Environment variable 'calcConnStr' not found.");
        return;
    }

    var uri = new Uri(uriString);
    var userInfo = uri.UserInfo.Split(':');

    var connectionString = $"Host={uri.Host};Username={userInfo[0]};Password={userInfo[1]};Database={uri.AbsolutePath.TrimStart('/')}";

    
    using (var connection = new NpgsqlConnection(connectionString))
    {
        var command = new NpgsqlCommand("INSERT INTO Records (First, Second, Operator, Result) VALUES (@First, @Second, @Operator, @Result)", connection);
        
        command.Parameters.AddWithValue("@First", record.First);
        command.Parameters.AddWithValue("@Second", record.Second);
        command.Parameters.AddWithValue("@Operator", record.Operator);
        command.Parameters.AddWithValue("@Result", record.Result);

        connection.Open();
        command.ExecuteNonQuery();
    }

    Console.WriteLine("Record saved to database.");
}

public class Record
{
    public double First { get; set; }
    public double Second { get; set; }
    public string Operator { get; set; }
    public double Result { get; set; }
}
namespace Calculator.Tests;

public class Tests
{
    private ICalculator _calculator;
    [SetUp]
    public void Setup()
    {
        _calculator = new Calculator();
    }

    [Test]
    public void Add()
    {
        var result = _calculator.Add(5, 5);
        Assert.That(result, Is.EqualTo(10));
    }
    [Test]
    public void Subtract()
    {
        var result = _calculator.Subtract(5, 5);
        Assert.That(result, Is.EqualTo(0));
    }
    [Test]
    public void Multiply()
    {
        var result = _calculator.Multiply(5, 5);
        Assert.That(result, Is.EqualTo(25));
    }
    [Test]
    public void Divide()
    {
        var result = _calculator.Divide(5, 5);
        Assert.That(result, Is.EqualTo(1));
    }
}
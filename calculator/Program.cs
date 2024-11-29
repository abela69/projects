namespace calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double num1, num2, result;
            string operation;
            string continueCalculation;

            do
            {
                Console.WriteLine("Enter the first number: ");
                while (!double.TryParse(Console.ReadLine(), out num1))
                {
                    Console.WriteLine("Invalid input! Please enter a valid number.");
                    Console.WriteLine("Enter the first number: ");
                }

                Console.Write("Enter an operator (+, -, *, /): ");
                operation = Console.ReadLine();

                Console.WriteLine("Enter the second number: ");
                while (!double.TryParse(Console.ReadLine(), out num2))
                {
                    Console.WriteLine("Invalid input! Please enter a valid number.");
                    Console.WriteLine("Enter the second number: ");
                }

                switch (operation)
                {
                    case "+":
                        result = num1 + num2;
                        Console.WriteLine($"Result: {num1} + {num2} = {result}");
                        break;

                    case "-":
                        result = num1 - num2;
                        Console.WriteLine($"Result: {num1} - {num2} = {result}");
                        break;

                    case "*":
                        result = num1 * num2;
                        Console.WriteLine($"Result: {num1} * {num2} = {result}");
                        break;

                    case "/":
                        if (num2 == 0)
                        {
                            Console.WriteLine("Can't divide by 0.");
                        }
                        else
                        {
                            result = num1 / num2;
                            Console.WriteLine($"Result: {num1} / {num2} = {result}");
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid operator! Please use +, -, *, or /.");
                        break;
                }

                Console.WriteLine("Do you want to perform another calculation? (yes/no): ");
                continueCalculation = Console.ReadLine().ToLower();

            } while (continueCalculation == "yes");
            Console.WriteLine("Thank you for using the calculator. Goodbye!");
            Console.ReadKey();
        }
    }
}

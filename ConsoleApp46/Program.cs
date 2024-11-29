namespace ConsoleApp46
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var atm = new ATM();
            atm.Run();
        }
    }
    class User
    {
        public string Name { get; set; }
        public int Pin { get; set; }
        public decimal Balance { get; set; }

        public User(string name, int pin, decimal balance)
        {
            Name = name;
            Pin = pin;
            Balance = balance;
        }
         public void SaveToFile()
        {
            string filepath = Name + ".txt";
            string userData = $"{Name},{Pin},{Balance}";
            File.WriteAllText(filepath, userData);
        }
        public static User LoadFromfile(string name)
        {
            string filepath = name + ".txt";
            if (File.Exists(filepath))
            {
                string userData = File.ReadAllText(filepath);
                string[] userParts = userData.Split(",");
                string username = userParts[0];
                int pin = int.Parse(userParts[1]);
                decimal balance = decimal.Parse(userParts[2]);
                return new User(username, pin, balance);
            }
            else
            {
                Console.WriteLine("user not found");
                return null;
            }
        }
        public bool Validatepin(int enteredPin)
        {
            return Pin == enteredPin;
        }

    }
    class ATM
    {
        private User CurrentUser;

        public void Run()
        {
            Console.WriteLine("Welcome to the ATM!");
            Console.Write("Please enter your name: ");
            string name = Console.ReadLine();

            CurrentUser = User.LoadFromfile(name);
            if (CurrentUser == null)
            {
                Console.WriteLine("Invalid name or account does not exist");
                return;
            }
            if (!AuthenticateUser())
            {
                return;
            }

            ShowMenu();
        }

        private bool AuthenticateUser()
        {
            const int maxAttempts = 3;

            for (int attempts = maxAttempts; attempts > 0; attempts--)
            {
                Console.Write("Please enter your 4-digit PIN: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int enteredPin) || input.Length != 4)
                {
                    Console.WriteLine("Incorrect input. Please enter a valid 4-digit PIN.");
                    continue;
                }

                if (CurrentUser.Validatepin(enteredPin))
                {
                    Console.WriteLine("Authentication successful");
                    return true;
                }

                Console.WriteLine($"Incorrect PIN. {attempts - 1} attempts remaining.");
            }

            Console.WriteLine("Too many failed attempts. Returning to main menu.");
            return false;
        }

        private void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\nATM Menu:");
                Console.WriteLine("1. Check Balance");
                Console.WriteLine("2. Deposit Money");
                Console.WriteLine("3. Withdraw Money");
                Console.WriteLine("4. Change PIN");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CheckBalance();
                        break;
                    case "2":
                        DepositMoney();
                        break;
                    case "3":
                        WithdrawMoney();
                        break;
                    case "4":
                        ChangePin();
                        break;
                    case "5":
                        Console.WriteLine("Thank you for using the ATM. Goodbye!");
                        CurrentUser.SaveToFile();
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void CheckBalance()
        {
            Console.WriteLine($"Your Balance: {CurrentUser.Balance:C}");
        }

        private void DepositMoney()
        {
            Console.Write("Enter the amount to deposit: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                CurrentUser.Balance += amount;
                Console.WriteLine($"Successfully deposited {amount:C}. New balance: {CurrentUser.Balance:C}");
            }
            else
            {
                Console.WriteLine("Invalid amount. Please try again.");
            }
        }

        private void WithdrawMoney()
        {
            Console.Write("Enter the amount to withdraw: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                if (amount <= CurrentUser.Balance)
                {
                    CurrentUser.Balance -= amount;
                    Console.WriteLine($"Successfully withdrew {amount:C}. New balance: {CurrentUser.Balance:C}");
                }
                else
                {
                    Console.WriteLine("Insufficient balance.");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount. Please try again.");
            }
        }

        private void ChangePin()
        {
            Console.Write("Enter your current 4-digit PIN: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int currentPin) && CurrentUser.Validatepin(currentPin))
            {
                Console.Write("Enter your new 4-digit PIN: ");
                string newPinInput = Console.ReadLine();

                if (int.TryParse(newPinInput, out int newPin) && newPinInput.Length == 4)
                {
                    CurrentUser.Pin = newPin; // Update the user's PIN
                    Console.WriteLine("PIN changed successfully!");
                    CurrentUser.SaveToFile(); // Save the updated user data to file
                }
                else
                {
                    Console.WriteLine("Invalid new PIN. Please enter a 4-digit number.");
                }
            }
            else
            {
                Console.WriteLine("Current PIN is incorrect.");
            }
        }
    }

}

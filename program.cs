using System;
using System.Collections.Generic;
using System.IO;

class ATM
{
    static void Main()
    {
        string csvFilePath = "C:\\Users\\flip1\\Downloads\\fisherbank.csv";
        var accountData = new Dictionary<string, (string Password, decimal Balance)>();
        using (var reader = new StreamReader(csvFilePath))
        {
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                if (values.Length == 3)
                {
                    string accountId = values[0];
                    string password = values[1];
                    decimal balance = decimal.Parse(values[2]);
                    accountData[accountId] = (password, balance);
                }
            }
        }

        Console.WriteLine("1 - Login");
        Console.WriteLine("2 - Register");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Login(accountData, csvFilePath);
                break;
            case "2":
                Register(accountData, csvFilePath);
                break;
            default:
                Console.WriteLine("Invalid choice. Please select a valid option.");
                break;
        }
    }

    static void Login(Dictionary<string, (string Password, decimal Balance)> accountData, string csvFilePath)
    {
        Console.WriteLine("Insert your Banking ID number: ");
        string inputAccountId = Console.ReadLine();
        Console.WriteLine("Insert your password: ");
        string inputPassword = Console.ReadLine();

        if (accountData.TryGetValue(inputAccountId, out var accountInfo) && accountInfo.Password == inputPassword)
        {
            Console.WriteLine("Login Successful!");
            ShowMenu(accountInfo.Balance, accountData, inputAccountId, csvFilePath);
        }
        else
        {
            Console.WriteLine("Unsuccessful Login! Invalid account ID or password.");
        }
    }

    static void Register(Dictionary<string, (string Password, decimal Balance)> accountData, string csvFilePath)
    {
        Console.WriteLine("Create a new Banking ID number: ");
        string newAccountId = Console.ReadLine();
        Console.WriteLine("Create a new password: ");
        string newPassword = Console.ReadLine();

        accountData[newAccountId] = (newPassword, 0);

        using (var writer = new StreamWriter(csvFilePath, append: true))
        {
            writer.WriteLine($"\n{newAccountId},{newPassword},0");
        }

        Console.WriteLine("Registration Successful!");
    }

    static void ShowMenu(decimal accountBalance, Dictionary<string, (string Password, decimal Balance)> accountData, string accountId, string csvFilePath)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1 - View Account Balance");
            Console.WriteLine("2 - Deposit Money");
            Console.WriteLine("3 - Withdraw Money");
            Console.WriteLine("4 - Logout");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine($"Your current account balance is: {accountBalance:C}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case "2":
                    Console.WriteLine("Enter the amount to deposit: ");
                    decimal depositAmount = decimal.Parse(Console.ReadLine());
                    accountBalance += depositAmount;
                    accountData[accountId] = (accountData[accountId].Password, accountBalance);
                    Console.WriteLine("Deposit successful!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case "3":
                    Console.WriteLine("Enter the amount to withdraw: ");
                    decimal withdrawAmount = decimal.Parse(Console.ReadLine());
                    if (withdrawAmount <= accountBalance)
                    {
                        accountBalance -= withdrawAmount;
                        accountData[accountId] = (accountData[accountId].Password, accountBalance);
                        Console.WriteLine("Withdrawal successful!");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient balance!");
                    }
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case "4":
                    WriteDataToCSV(accountData, csvFilePath);
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        }
    }

    static void WriteDataToCSV(Dictionary<string, (string Password, decimal Balance)> accountData, string csvFilePath)
    {
        using (var writer = new StreamWriter(csvFilePath))
        {
            writer.WriteLine("Ids,Passwords,Balances");
            foreach (var account in accountData)
            {
                writer.WriteLine($"{account.Key},{account.Value.Password},{account.Value.Balance}");
            }
        }
    }
}

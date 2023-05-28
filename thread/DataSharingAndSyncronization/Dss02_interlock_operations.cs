public class Dss02_interlock_operations
{
    class BankAccount
    {
        private int _balance;
        public int Balance { get => _balance; private set => _balance = value; }

        public void Deposit(int amount)
        {
            Interlocked.Add(ref _balance, amount);
        }

        public void Withdraw(int amount)
        {
            Interlocked.Add(ref _balance, -amount);
        }
    }

    public static void Run()
    {
        var ba = new BankAccount();
        var tasks = new List<Task>();

        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    ba.Deposit(1);
                }
            }));

            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    ba.Withdraw(1);
                }
            }));

        }
        Console.WriteLine($"Interlock. Final balance is {ba.Balance}.");
    }
}
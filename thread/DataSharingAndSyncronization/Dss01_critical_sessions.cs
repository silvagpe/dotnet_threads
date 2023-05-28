public class Dss01_critical_sessions
{
    class BankAccount
    {
        public object padLock = new object();
        public int Balance { get; private set; }

        public void Deposit(int amount)
        {
            lock (padLock)
            {
                Balance += amount;
            }
        }

        public void Withdraw(int amount)
        {
            lock (padLock)
            {
                Balance -= amount;
            }
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

        Console.WriteLine($"Final balance is {ba.Balance}.");

    }

}
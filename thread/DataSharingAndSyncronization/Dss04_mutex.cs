public class Dss04_mutex
{
    class BankAccount
    {
        public int Balance { get; private set; }

        public void Deposit(int amount)
        {
            Balance += amount;
        }

        public void Withdraw(int amount)
        {
            Balance -= amount;
        }

        public void Transfer(BankAccount where, int amount)
        {
            Balance -= amount;
            where.Balance += amount;
        }
    }

    public static void Run()
    {
        var ba = new BankAccount();
        var ba2 = new BankAccount();
        var tasks = new List<Task>();

        var mutex = new Mutex();
        var mutex2 = new Mutex();

        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    bool haveLock = mutex.WaitOne();
                    try
                    {
                        ba.Deposit(1);
                    }
                    finally
                    {
                        if (haveLock) mutex.ReleaseMutex();
                    }
                }
            }));

            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    bool haveLock = mutex2.WaitOne();
                    try
                    {
                        ba2.Deposit(1);
                    }
                    finally
                    {
                        if (haveLock) mutex2.ReleaseMutex();
                    }
                }
            }));

            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    bool haveLock = WaitHandle.WaitAll(new[] { mutex, mutex2 });
                    try
                    {
                        ba.Transfer(ba2, 1);
                    }
                    finally
                    {
                        if (haveLock)
                        {
                            mutex.ReleaseMutex();
                            mutex2.ReleaseMutex();
                        }
                    }
                }
            }));
        }

        Console.WriteLine($"Mutex.");
        Console.WriteLine($"Final balance is Account 1 {ba.Balance}");
        Console.WriteLine($"Final balance is Account 2 {ba2.Balance}");
    }
}
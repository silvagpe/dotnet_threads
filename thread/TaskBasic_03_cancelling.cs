public class TaskBasic_03_cancelling
{
    private static void SoftCancelation()
    {
        var cst = new CancellationTokenSource();
        var token = cst.Token;
        var task = new Task(() =>
        {
            int i = 0;
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }
                else
                {
                    Console.Write($"{i++} \t");
                }
            }
        }, token);
        task.Start();

        Console.ReadKey();
        cst.Cancel();
        Console.WriteLine("Task is canceled");
    }

    private static void ThrowCancelation()
    {
        var cst = new CancellationTokenSource();
        var token = cst.Token;
        var task = new Task(() =>
        {
            int i = 0;
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }
                else
                {
                    Console.Write($"{i++} \t");
                }
            }
        }, token);
        task.Start();

        Console.ReadKey();
        cst.Cancel();
        Console.WriteLine("Task is canceled");
    }

    private static void ThrowIfCancellationRequested()
    {
        var cst = new CancellationTokenSource();
        var token = cst.Token;

        token.Register(() => {
            Console.WriteLine("Cancelation has been requested");
        });

        var task = new Task(() =>
        {
            int i = 0;
            while (true)
            {
                token.ThrowIfCancellationRequested();
                Console.Write($"{i++} \t");
            }
        }, token);
        task.Start();

        Task.Factory.StartNew(() =>{
            token.WaitHandle.WaitOne();
            Console.WriteLine("Wait handle released, cancelation was requested");
        });

        Console.ReadKey();
        cst.Cancel();
        Console.WriteLine("Task is canceled");
    }

    private static void MultiTokenCancelation()
    {
        var planned = new CancellationTokenSource();
        var preventative = new CancellationTokenSource();
        var emergency = new CancellationTokenSource();

        var paranoid = CancellationTokenSource.CreateLinkedTokenSource(
            planned.Token, preventative.Token, emergency.Token
        );

        Task.Factory.StartNew(()=>{
            int i = 0;
            while (true)
            {
                paranoid.Token.ThrowIfCancellationRequested();
                Console.Write($"{i++}\t");
                Thread.Sleep(1000);
            }
        }, paranoid.Token);

        Console.ReadKey();
        emergency.Cancel();

    }

    public static void Run()
    {
        MultiTokenCancelation();
    }
}
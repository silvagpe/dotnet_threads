public class TaskBasic_05_waiting_tasks
{

    private static void TaskWait()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var task = new Task(() =>
        {
            Console.WriteLine("I take 5 seconds");
            for (int i = 0; i < 6; i++)
            {
                token.ThrowIfCancellationRequested();
                Thread.Sleep(1000);
            }
            Console.WriteLine($"Task 1: {Task.CurrentId}");
        }, token);
        task.Start();

        //Para esperar apenas uma task ser completada
        //task.Wait(token);

        var task2 = Task.Factory.StartNew(() =>
        {
            Thread.Sleep(3000);
            Console.WriteLine($"Task 2: {Task.CurrentId}");
        });

        //Espera todas as task serem completadas
        //Task.WaitAll(new []{task, task2});

        //Espera a primeira task ser concluída
        //Task.WaitAny(new[] { task, task2 });

        Task.WaitAll(new[] { task, task2 }, 4000, token);
        cts.Cancel();
        Console.WriteLine("Timeout");
        Console.WriteLine($"Task 1, status: {task.Status}");
        Console.WriteLine($"Task 2, status: {task2.Status}");
        
    }


    public static void Run()
    {
        TaskWait();

    }
}
public class TaskBasic_02_get_result
{
    private static int TextLength(object o){

        Console.WriteLine($"Task with id {Task.CurrentId} processing object {o} ...");
        return o.ToString().Length;
    }

    public static void Run(){

        object text1 = "testing", text2 = "this";

        Task<int> task1 = new Task<int>(TextLength,text1);
        task1.Start();

        Task<int> task2 = Task.Factory.StartNew(TextLength, text2);

        Console.WriteLine($"Length of '{text1}' is {task1.Result}");
        Console.WriteLine($"Length of '{text2}' is {task2.Result}");
        
    }
    
}
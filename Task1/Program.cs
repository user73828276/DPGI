namespace Task1
{
    public class Program
    {
        public static void Main()
        {
            var hClass = new HelloWorld();

            hClass.Print();
        }
    }
    public class HelloWorld
    {
        public void Print()
        {
            Console.WriteLine("Hello, World!");
        }
    }
}

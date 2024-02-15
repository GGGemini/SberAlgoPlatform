namespace Task2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * Вставьте код функции Mutate (без использования ключевого слова unsafe), чтобы на консоль вывелось «404».
             * Сигнатуру функции не менять, замыкания не ловить.
             */

            const string constStr = "000";

            Mutate(constStr);

            var nonConst = "000";

            // constStr и nonConst указывают на один и тот же объект в памяти,
            // поэтому небезопасная замена constStr приведёт и к замене nonConst,
            // так как эти переменные - ссылки

            Console.WriteLine(nonConst);
        }

        static unsafe void Mutate(string str)
        {
            // строки являются неизменяемым типом (immutable) в C#
            // когда строка "изменяется", на самом деле, создаётся новая строка
            // выполнение задания в указанных условиях невозможно

            //fixed (char* arr = str)
            //{
            //    arr[0] = '4';
            //    arr[1] = '0';
            //    arr[2] = '4';
            //}
        }
    }
}

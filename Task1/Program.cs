namespace Task1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * Почему компилятор запрещает нам писать такой код? Что бы могло пойти не так, если бы он разрешал?
             * private Func<int, bool> BuildPredicate(ref int threshold) => x => x >= threshold;
             */

            // 1-ая причина - непредсказуемость поведения программы из-за возможности
            // изменения переменной уже после объявления делегата
            int i = 5;

            var predicate = BuildPredicate(i);
            var isGreaterBefore = predicate(7); // ожидается true, т. к. 7 больше 5
            i = 10;
            var isGreaterAfter = predicate(7); // тоже true, но могло бы быть false - непредсказуемо

            // если бы мы передали число i в качестве ссылки, то это вызвало бы непредсказуемое поведение в момент выполнения предиката
            // лучше просто создать два разных предиката и вызвать их для получения результатов

            var isGreaterOrEqualsFive = BuildPredicate(5)(7); // true
            var isGreaterOrEqualsTen = BuildPredicate(10)(7); // false

            // 2-ая причина - возможное отсутствие значения в момент вызова предиката
            int j = 7;

            Func<int, bool>? predicate2;
            if (j > 7)
            {
                int a = 1;
                predicate2 = BuildPredicate(a);
            }
            else
            {
                int a = 2;
                predicate2 = BuildPredicate(a);
            }

            // если бы переменная a - являлась ссылочным типом, то было бы не понятно,
            // где брать значение, ведь переменная уже не в нашей области видимости,
            // а так - скопированное значение уже осталось в переменной predicate2
            var isGreater2 = predicate2(1);
        }

        static Func<int, bool> BuildPredicate(int treshould)
        {
            return x => x >= treshould;
        }

        // то же самое (взаимозаменяемо)
        static Predicate<int> BuildPredicate2(int treshould)
        {
            return x => x >= treshould;
        }
    }
}

using System.Collections.Generic;


namespace KSRecs.Utils
{
    public static class Python
    {
        public static IEnumerable<Element<T>> Enumerate<T>(IEnumerable<T> first)
        {
            int counter = 0;
            var First = first.GetEnumerator();
            while (First.MoveNext())
            {
                yield return new Element<T>(counter, First.Current);
                counter++;
            }
            yield break;
        }

        public static Dictionary<TKey, TValue> ZipDict<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values)
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            var Keys = keys.GetEnumerator();
            var Values = values.GetEnumerator();
            while (Keys.MoveNext() && Values.MoveNext())
            {
                dict.Add(Keys.Current, Values.Current);
            }

            return dict;
        }

        public static IEnumerable<Bundle<T1, T2>> Zip<T1, T2>(IEnumerable<T1> first, IEnumerable<T2> second)
        {
            int counter = 0;
            var First = first.GetEnumerator();
            var Second = second.GetEnumerator();
            while (true)
            {
                if (First.MoveNext() && Second.MoveNext())
                {
                    yield return new Bundle<T1, T2>(counter, First.Current, Second.Current);
                }
                else
                {
                    break;
                }

                counter++;
            }

            yield break;
        }

        public static IEnumerable<Bundle<T1, T2, T3>> Zip<T1, T2, T3>(IEnumerable<T1> first, IEnumerable<T2> second, IEnumerable<T3> third)
        {
            int counter = 0;
            var First = first.GetEnumerator();
            var Second = second.GetEnumerator();
            var Third = third.GetEnumerator();
            while (true)
            {
                if (First.MoveNext() && Second.MoveNext() && Third.MoveNext())
                {
                    yield return new Bundle<T1, T2, T3>(counter, First.Current, Second.Current, Third.Current);
                }
                else
                {
                    break;
                }
                counter++;
            }

            yield break;
        }

        public static IEnumerable<Bundle<T1, T2, T3, T4>> Zip<T1, T2, T3, T4>(IEnumerable<T1> first, IEnumerable<T2> second, IEnumerable<T3> third, IEnumerable<T4> forth)
        {
            int counter = 0;
            var First = first.GetEnumerator();
            var Second = second.GetEnumerator();
            var Third = third.GetEnumerator();
            var Forth = forth.GetEnumerator();
            while (true)
            {
                if (First.MoveNext() && Second.MoveNext() && Third.MoveNext() && Forth.MoveNext())
                {
                    yield return new Bundle<T1, T2, T3, T4>(counter, First.Current, Second.Current, Third.Current, Forth.Current);
                }
                else
                {
                    break;
                }
                counter++;
            }

            yield break;
        }

        public static IEnumerable<Bundle<T1, T2, T3, T4, T5>> Zip<T1, T2, T3, T4, T5>(IEnumerable<T1> first, IEnumerable<T2> second, IEnumerable<T3> third, IEnumerable<T4> forth, IEnumerable<T5> fifth)
        {
            int counter = 0;
            var First = first.GetEnumerator();
            var Second = second.GetEnumerator();
            var Third = third.GetEnumerator();
            var Forth = forth.GetEnumerator();
            var Fifth = fifth.GetEnumerator();
            while (true)
            {
                if (First.MoveNext() && Second.MoveNext() && Third.MoveNext() && Forth.MoveNext())
                {
                    yield return new Bundle<T1, T2, T3, T4, T5>(counter, First.Current, Second.Current, Third.Current, Forth.Current, Fifth.Current);
                }
                else
                {
                    break;
                }
                counter++;
            }

            yield break;
        }

        public static int PythonToCsIndex<T>(List<T> list, int index)
        {
            if (index < 0) return list.Count + index;
            return index;
        }
        
        public static int PythonToCsIndex<T>(T[] list, int index)
        {
            if (index < 0) return list.Length + index;
            return index;
        }
        
        public static IEnumerable<T> SubEnumerator<T>(List<T> combinations, int start, int end, int step)
        {
            int startIndex = PythonToCsIndex(combinations, start);
            int endIndex = PythonToCsIndex(combinations, end);
            int currentIndex = 0;
            
            foreach (T element in combinations)
            {
                if (currentIndex >= endIndex) yield break;
                if ((currentIndex - startIndex) % step == 0)
                {
                    yield return element;
                }
                currentIndex++;
            }
        }
        
        public static IEnumerable<T> SubEnumerator<T>(List<T> combinations, int start, int end)
        {
            int startIndex = PythonToCsIndex(combinations, start);
            int endIndex = PythonToCsIndex(combinations, end);
            int currentIndex = 0;
            
            foreach (T element in combinations)
            {
                if (currentIndex < startIndex) continue;
                if (currentIndex >= endIndex) yield break;
                
                yield return element;
                currentIndex++;
            }
        }
        
        public static IEnumerable<T> SubEnumerator<T>(T[] combinations, int start, int end, int step)
        {
            int startIndex = PythonToCsIndex(combinations, start);
            int endIndex = PythonToCsIndex(combinations, end);
            int currentIndex = 0;
            
            foreach (T element in combinations)
            {
                if (currentIndex >= endIndex) yield break;
                if ((currentIndex - startIndex) % step == 0)
                {
                    yield return element;
                }
                currentIndex++;
            }
        }
        
        public static IEnumerable<T> SubEnumerator<T>(T[] combinations, int start, int end)
        {
            int startIndex = PythonToCsIndex(combinations, start);
            int endIndex = PythonToCsIndex(combinations, end);
            int currentIndex = 0;
            
            foreach (T element in combinations)
            {
                if (currentIndex < startIndex) continue;
                if (currentIndex >= endIndex) yield break;
                
                yield return element;
                currentIndex++;
            }
        }
    }
    
    public struct Element<T>
    {
        public int index;
        public T Item;

        public Element(int index, T item)
        {
            this.index = index;
            this.Item = item;
        }
    }

    
    public struct Bundle<T1, T2>
    {
        public int index;
        public T1 Item1;
        public T2 Item2;

        public Bundle(int index, T1 item1, T2 item2)
        {
            this.index = index;
            this.Item1 = item1;
            this.Item2 = item2;
        }
    }


    public struct Bundle<T1, T2, T3>
    {
        public int index;
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;

        public Bundle(int index, T1 item1, T2 item2, T3 item3)
        {
            this.index = index;
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
        }
    }


    public struct Bundle<T1, T2, T3, T4>
    {
        public int index;
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;

        public Bundle(int index, T1 item1, T2 item2, T3 item3, T4 item4)
        {
            this.index = index;
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
        }
    }


    public struct Bundle<T1, T2, T3, T4, T5>
    {
        public int index;
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;

        public Bundle(int index, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            this.index = index;
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
        }
    }
}
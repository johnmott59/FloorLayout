using System.Collections.Generic;

namespace Edit2DLib
{
    public static class ListExtension
    {
        /*
         * This extension method for lists will let us write code that runs in C# and in javascript that will use a List<T> class.
         * We create a custom List<T> class for the Javascript code.
         * 
         * In Javascript we can't overload the [] operator so the List<T> class has an array, called 'items', that is used
         * to store the data, so you end up with references like 
         * List<T> thingList;
         * thingList.Add(something);
         * T thing = thingList.items[0];
         * 
         * We want to use the same syntax in C# and in Javascript so that the code can be sent into TypeScript without change
         * An exention method on List lets us access a list.items[0] field
         */
     //   public static T[] items<T>(this List<T> list)
     //   {
     //       return list.ToArray<T>();
     //   }

        // This method will let us have a cross platform mechanism for getting an item from a list.
        // in .net you can use an array reference, but on javascript you can't use an array reference for a class,
        // no operator overloading. 
        // we can expose an array in .net to allow an array reference but its not a property, its a method,
        // so the references look like items()[], which have to be post processed. Its ok to have post processing
        // but if there is a way to avoid it its best, just one less thing to go wrong

        public static T GetFrom<T>(this List<T> list,int index)
        {
            return list[index];
        }

        //
        // We need a way to assign a value to the list in .NET because the items in the list are fetched
        // via the items() method above, and you can't assing a value through the returned array
        // This syntax works fine in Javascript because the array reference is direct; 'items' is the array
        // So this method will simply assign a value to an index, which would normally be done in .net with
        // an array reference, but this portable code will work both .NET and javascript implemented in the List<T>
        // class in javascript
        //
        public static void AssignTo<T>(this List<T> list,int index, T value)
        {
            list[index] = value;
            return;
        }

    }
}

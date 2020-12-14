using BenchmarkDotNet.Attributes;

namespace Hw10
{
    public class MyClass
    {
        public override string ToString()
        {
            return "1";
        }
    }

    public class Benchmarks
    {
        public void Cycle(object arg)
        {
            var currentString = arg.ToString();
            for (int i = 0; i < 5; i++)
            {
                currentString += "1";
            }
        }

        //Профилировать по времени выполнения и по памяти обычные, виртуальные,
        //статические, generic-методы, dynamic, reflection. Что быстрее, на сколько?
        public void SimpleMethod()
        {
            Cycle("1");
        }

        public virtual void VirtualMethod()
        {
            Cycle("1");
        }

        public static void StaticMethod()
        {
            var currentString = "1";
            for (int i = 0; i < 5; i++)
            {
                currentString += "1";
            }
        }

        public void GenericMethod<T>(T currentString)
            where T : MyClass
        {
            Cycle(currentString.ToString());
        }

        public void DynamicMethod(dynamic dynamicObj)
        {
           Cycle(dynamicObj.ToString());
        }

        public void ReflectionMethod()
        {
            this.GetType()
                .GetMethod("Cycle")
                .Invoke(new MyClass(), new[] {new MyClass(), });
        }

        [Benchmark(Description = "SimpleMethod")]
        public void InvokeSimpleMethod()
        {
            VirtualMethod();
        }

        [Benchmark(Description = "VirtualMethod")]
        public void InvokeVirtualMethod()
        {
            VirtualMethod();
        }

        [Benchmark(Description = "StaticMethod")]
        public void InvokeStaticMethod()
        {
            StaticMethod();
        }

        [Benchmark(Description = "GenericMethod")]
        public void InvokeGenericMethod()
        {
            GenericMethod(new MyClass());
        }

        [Benchmark(Description = "DynamicMethod")]
        public void InvokeDynamicMethod()
        {
           DynamicMethod(new MyClass());
        }

        [Benchmark(Description = "ReflectionMethod")]
        public void InvokeReflectionMethod()
        {
            ReflectionMethod();
        }
    }
}
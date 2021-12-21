using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ExtractorSharp.Core.Emit {
    internal class Program {
        private static void Main(string[] args) {
            var command = Proxy.Create<Command>();
            command.Execute();

            Console.WriteLine("Hi, Dennis, great, we got the interceptor works.");
            Console.ReadLine();
        }
    }

    public class Command {
        public virtual void Execute() {
            Console.WriteLine("Command executing...");
            Console.WriteLine("Hello Kitty!");
            Console.WriteLine("Command executed.");
        }
    }

    public class Interceptor {
        public object Invoke(object obj, string method, object[] parameters) {
            Console.WriteLine(
              string.Format("Interceptor does something before invoke [{0}]...", method));

            var retObj = obj.GetType().GetMethod(method).Invoke(obj, parameters);

            Console.WriteLine(
              string.Format("Interceptor does something after invoke [{0}]...", method));

            return retObj;
        }
    }

    public class Proxy {
        public static T Create<T>() where T : class, new() {
            var baseType = typeof(T);

            var nameOfAssembly = baseType.Name + "ProxyAssembly";
            var nameOfModule = baseType.Name + "ProxyModule";
            var nameOfType = baseType.Name + "Proxy";

            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(nameOfAssembly), AssemblyBuilderAccess.Run);
            var moduleBuilder = assembly.DefineDynamicModule(nameOfModule);

            var typeBuilder = moduleBuilder.DefineType(nameOfType, TypeAttributes.Public, baseType);

            InjectInterceptor<T>(typeBuilder);

            var t = typeBuilder.CreateType();

            return Activator.CreateInstance(t) as T;
        }

        private static void InjectInterceptor<T>(TypeBuilder typeBuilder) {
            // ---- define fields ----

            var fieldInterceptor = typeBuilder.DefineField("_interceptor", typeof(Interceptor), FieldAttributes.Private);

            // ---- define costructors ----

            var constructorBuilder = typeBuilder.DefineConstructor(
              MethodAttributes.Public, CallingConventions.Standard, null);
            var ilOfCtor = constructorBuilder.GetILGenerator();

            ilOfCtor.Emit(OpCodes.Ldarg_0);
            ilOfCtor.Emit(OpCodes.Newobj, typeof(Interceptor).GetConstructor(new Type[0]));
            ilOfCtor.Emit(OpCodes.Stfld, fieldInterceptor);
            ilOfCtor.Emit(OpCodes.Ret);

            // ---- define methods ----

            var methodsOfType = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance);

            for(var i = 0; i < methodsOfType.Length; i++) {
                var method = methodsOfType[i];
                var methodParameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();

                var methodBuilder = typeBuilder.DefineMethod(
                  method.Name,
                  MethodAttributes.Public | MethodAttributes.Virtual,
                  CallingConventions.Standard,
                  method.ReturnType,
                  methodParameterTypes);

                var ilOfMethod = methodBuilder.GetILGenerator();
                ilOfMethod.Emit(OpCodes.Ldarg_0);
                ilOfMethod.Emit(OpCodes.Ldfld, fieldInterceptor);

                // create instance of T
                ilOfMethod.Emit(OpCodes.Newobj, typeof(T).GetConstructor(new Type[0]));
                ilOfMethod.Emit(OpCodes.Ldstr, method.Name);

                // build the method parameters
                if(methodParameterTypes == null) {
                    ilOfMethod.Emit(OpCodes.Ldnull);
                } else {
                    var parameters = ilOfMethod.DeclareLocal(typeof(object[]));
                    ilOfMethod.Emit(OpCodes.Ldc_I4, methodParameterTypes.Length);
                    ilOfMethod.Emit(OpCodes.Newarr, typeof(object));
                    ilOfMethod.Emit(OpCodes.Stloc, parameters);

                    for(var j = 0; j < methodParameterTypes.Length; j++) {
                        ilOfMethod.Emit(OpCodes.Ldloc, parameters);
                        ilOfMethod.Emit(OpCodes.Ldc_I4, j);
                        ilOfMethod.Emit(OpCodes.Ldarg, j + 1);
                        ilOfMethod.Emit(OpCodes.Stelem_Ref);
                    }
                    ilOfMethod.Emit(OpCodes.Ldloc, parameters);
                }

                // call Invoke() method of Interceptor
                ilOfMethod.Emit(OpCodes.Callvirt, typeof(Interceptor).GetMethod("Invoke"));

                // pop the stack if return void
                if(method.ReturnType == typeof(void)) {
                    ilOfMethod.Emit(OpCodes.Pop);
                }

                // complete
                ilOfMethod.Emit(OpCodes.Ret);
            }
        }
    }
}
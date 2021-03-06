﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleApplication1.ServiceReference1;
using System.Runtime.Remoting.Messaging;

namespace ConsoleApplication1
{
    class Program
    {
        private static Service1Client client;

        static void Main(string[] args)
        {
            client = new Service1Client();

            CompositeType customType = new CompositeType
            {
                BoolValue = true,
                StringValue = "Sample string"
            };

            /*For targetFramework starting from 3.5 you can have the following approach
             1. When adding the service reference, enable Asynchronous features.
             2. Then the Service client will get additional methods and events which can be used for Async operations.
             3. GetDataCompleted is Eventhandler to which we can assign our method, which will be executed once the service
                operation completes
             4. GetDataAsyn is another additional method generated, when Async operations are enabled.
             */
            client.GetDataUsingDataContractCompleted += new EventHandler<GetDataUsingDataContractCompletedEventArgs>(client_GetDataCompleted);
            client.GetDataUsingDataContractAsync(customType);
            Console.WriteLine("Service operation called. waiting for result...");

            Console.ReadLine();

            /*
             If targetFramework is lower than 3.5 you may need to used the following feature.
             1. Here also you have to enable Async features while adding service reference.
             2. Then we will get Begin and End Methods for Our OpearationContracts.
             3. Calling the Begin Method will Initialze the Async operation. This method will return
               an AsyncResult object.
             4.Now you can submit this AsyncResult to your End method which will then return the actual result from the
              Service Method
             5. If you want to have 
             */
            CompositeType customType1 = new CompositeType
            {
                BoolValue = true,
                StringValue = "Sample string"
            };
            IAsyncResult promise = client.BeginGetDataUsingDataContract(customType1, new AsyncCallback(MyAsyncCallback), null);
            Console.WriteLine("Service operation called. waiting for result...");
            Console.WriteLine("Here comes the tasks to be done before service method completes");
            //string data = client.EndGetData(promise);
            //Console.WriteLine("Service operation completed and Result from EndGetData" + data);
            Console.ReadLine();

        }

        static void client_GetDataCompleted(object sender, GetDataUsingDataContractCompletedEventArgs e)
        {
            Console.WriteLine("Service operation completed and Result from service..." + e.Result);
            Console.ReadLine();

        }

        static void MyAsyncCallback(IAsyncResult asyncResult)
        {
            var getDataResult = client.EndGetDataUsingDataContract(asyncResult);
            Console.WriteLine("Service operation completed from MyAsyncCallback from service..." + getDataResult);
            Console.ReadLine();
        }

    }
}

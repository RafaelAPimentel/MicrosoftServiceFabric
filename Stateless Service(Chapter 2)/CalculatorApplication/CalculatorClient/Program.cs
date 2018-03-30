﻿using System;
using System.Fabric;
using System.ServiceModel;
using CalculatorService;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;

namespace CalculatorClient
{

    class Program
    {
        static void Main(string[] args)
        {
            //while (true)
            //{
            //    var calculatorClient = ServiceProxy.Create<ICalculatorService>
            //    (new Uri("fabric:/CalculatorApplication/CalculatorService"));
            //    var result = calculatorClient.Add(1, 2).Result;
            //    Console.WriteLine(result);
            //    Thread.Sleep(3000);
            //}

            Uri ServiceName = new Uri("fabric:/CalculatorApplication/CalculatorService");
            ServicePartitionResolver serviceResolver = new ServicePartitionResolver(() =>
                new FabricClient());
            NetTcpBinding binding = CreateClientConnectionBinding();
            Client calClient = new Client(new WcfCommunicationClientFactory<ICalculatorService>(binding,null,serviceResolver), ServiceName);
            Console.WriteLine(calClient.Add(3, 5).Result);
            Console.ReadKey();
        }

        private static NetTcpBinding CreateClientConnectionBinding()
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None)
            {
                SendTimeout = TimeSpan.MaxValue,
                ReceiveTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.FromSeconds(5),
                CloseTimeout = TimeSpan.FromSeconds(5),
                MaxReceivedMessageSize = 1024 * 1024
            };
            binding.MaxBufferSize = (int)binding.MaxReceivedMessageSize;
            binding.MaxBufferPoolSize = Environment.ProcessorCount * binding.MaxReceivedMessageSize;
            return binding;
        }
    }
}

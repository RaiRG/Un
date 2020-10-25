﻿using System;
using System.Linq;
using System.Net;

namespace Hw7
{
    class Client
    {
        public static string path = "https://localhost:5001/?expression=";

        static void Main(string[] args)
        {
            Console.WriteLine("Введите выражение:");
            var inputData = Console.ReadLine().Split().ToArray();

            //Для корректного Url
            for (var index = 0; index < inputData.Length; index++)
            {
                inputData[index] = inputData[index] switch
                {
                    "+" => "%2B",
                    "%2F" => "%2F",
                    _ => inputData[index]
                };
            }

            var url = path + inputData;
            var req = HttpWebRequest.Create(url);
            var response = (HttpWebResponse) req.GetResponse();
            // Проверяем ответ
            var statusCode = Convert.ToInt32(response.StatusCode);
            string result;
            if (statusCode == 200)
                result = response.Headers["result"];
            else
                result = "Ошибка";
            
        }
    }
}
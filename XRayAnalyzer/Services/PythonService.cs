using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model;
using XRayAnalyzer.MVVM.Model.Communication;
using XRayAnalyzer.Objects.Enums;

namespace XRayAnalyzer.Services
{
    class PythonService
    {
        private static readonly Lazy<PythonService> lazySingleton = new (() => new PythonService());
        public static PythonService Instance { get { return lazySingleton.Value; } }

        private ProcessStartInfo ProcessStartInfo { get; set; }
        private Process Process { get; set; }
        private StreamWriter? StandardInput { get; set; }
        private TaskCompletionSource<Response> Response { get; set; }
        public bool EnableLoging { get; set; } = true;
        private bool IsRunning { get; set; } = false;

        public PythonService(string? fileName = @".\Python\python.exe", string? scriptPath = "\"./Scripts/scripts.py\"")
        {
            Response = new TaskCompletionSource<Response>();

            ProcessStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = scriptPath,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true
            };

            Process = new Process
            {
                StartInfo = ProcessStartInfo
            };
            Process.OutputDataReceived += (sender, args) =>
            {
                Loging("Output data received" + Environment.NewLine + args.Data, LogType.Success);
                _ = Response.TrySetResult(new Response(ResponseType.Success, args.Data));
            };
            Process.ErrorDataReceived += (sender, args) =>
            {
                Loging("Error data received" + Environment.NewLine + args.Data, LogType.Error);
                _ = Response.TrySetResult(new Response(ResponseType.Error, args.Data));
            };
        }

        public bool Run()
        {
            try
            {
                Loging("Process starting");
                _ = Process.Start();

                StandardInput = Process.StandardInput;

                Loging("Begin output read line" + Environment.NewLine + "Begin error read line");
                Process.BeginOutputReadLine();
                Process.BeginErrorReadLine();
                IsRunning = true;
            }
            catch (Exception ex)
            {
                Loging(ex.Message, LogType.Error);
                IsRunning = false;
            }
            return IsRunning;
        }

        private void Loging(string message, LogType logType = LogType.Info)
        {
            if (EnableLoging)
            {
                LogingService.Instance.AddMessage(message, logType);
            }
        }

        public async Task<Response> GetResponseAsync()
        {
            _ = await Response.Task;
            Response response = Response.Task.Result.Clone();
            Response = new TaskCompletionSource<Response>();
            return response;
        }

        public Response GetResponse()
        {
            Response.Task.Wait();
            Response response = Response.Task.Result.Clone();
            Response = new TaskCompletionSource<Response>();
            return response;
        }

        public string WriteLine(string? argument, string? parameters)
        {
            string input = argument + " " + parameters;
            Loging("Standard input write line" + Environment.NewLine + input);
            StandardInput?.WriteLine(input);
            return input;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1">Properties type</typeparam>
        /// <typeparam name="T2">Response type</typeparam>
        /// <param name="properties">Properties for python request</param>
        /// <returns></returns>
        public T2? GetData<T1, T2>(T1 properties) where T1 : IPropertiesBaseInterface
        {
            if (IsRunning)
            {
                JsonSerializerOptions options = new ()
                {
                    IgnoreReadOnlyProperties = true,
                    WriteIndented = false
                };
                _ = WriteLine($"-f {properties.FunctionName} -p", JsonSerializer.Serialize(properties, options));
                Response response = GetResponse();
                return response.Type == ResponseType.Success ? JsonSerializer.Deserialize<T2>(response.Message) : default;
            }
            else return default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1">Properties type</typeparam>
        /// <typeparam name="T2">Response type</typeparam>
        /// <param name="properties">Properties for python request</param>
        /// <returns></returns>
        public async Task<T2?> GetDataAsync<T1, T2>(T1 properties) where T1 : IPropertiesBaseInterface
        {
            if (IsRunning)
            {
                JsonSerializerOptions options = new()
                {
                    IgnoreReadOnlyProperties = true,
                    WriteIndented = false
                };
                _ = WriteLine($"-f {properties.FunctionName} -p", JsonSerializer.Serialize(properties, options));
                Response response = await GetResponseAsync();
                return response.Type == ResponseType.Success ? JsonSerializer.Deserialize<T2>(response.Message) : default;
            }
            else return default;
        }
    }
}

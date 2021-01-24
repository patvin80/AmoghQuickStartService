using System;
using System.IO;
using System.Timers;
using System.Dynamic;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Skylight.Model;
using Skylight.Sdk;

namespace quickStartService
{
    class Program : ISessionEventListener
    {
        static Manager _manager;

        static async Task Main(string[] args)
        {
            _manager = new Manager(new Program());
            // If your domain is in the EU update both the API and MQTT with .eu
            // both should read .skylight.eu.upskill.io
            string username = System.Environment.GetEnvironmentVariable("SKYLIGHT_USERNAME");
            string password = System.Environment.GetEnvironmentVariable("SKYLIGHT_PASSWORD");
            string domain = System.Environment.GetEnvironmentVariable("SKYLIGHT_DOMAIN");

            _manager.SetCredentials(username, password, domain, "https://api.skylight.upskill.io", "ssl://mqtt.skylight.upskill.io");
            _manager.SetVerbosity(Logger.Verbosity.Verbose);

            await _manager.Connect();
            _manager.KeepAlive();
        }

        async Task ISessionEventListener.OnSessionCreated(SessionEventBase sessionEvent)
        {
            Console.WriteLine("Session Created - Accessing Procedure");

            //Create a new set of data to simulate our process
            AssemblySessionData data = new AssemblySessionData();
            data.processName = "Process XYZ - From External System";

            //Add some steps to the process
            data.steps = new List<AssemblyStep>();
            data.steps.Add(new AssemblyStep("1", "Prepare Components X and Y"));
            data.steps.Add(new AssemblyStep("2", "Prepare Component Z"));
            data.steps.Add(new AssemblyStep("3", "Bolt Component Z onto Component Y"));
            data.steps.Add(new AssemblyStep("4", "Adjust Ring Adapter of Component X to accept ZY subassembly"));
            data.steps.Add(new AssemblyStep("5", "Weld ZY subassembly onto Component X"));

            //Since this is a new run-through, each step will be incomplete
            data.stepStatus = new Dictionary<string, string>();
            for (int i = 0; i < 5; i++)
            {
                data.stepStatus.Add(i.ToString(), "incomplete");
            }

            //Update the new session's data
            await _manager.PatchSessionData(sessionEvent.ApplicationId, sessionEvent.SessionId, data);
        }

        async Task ISessionEventListener.OnSessionPropertiesUpdated(SessionEventBase sessionEvent)
        {
            Console.WriteLine("Session properties updated: " + sessionEvent.SessionId);
            var properties = sessionEvent.Properties;
            foreach (KeyValuePair<string, string> kvp in properties)
            {
                Console.WriteLine("{" + kvp.Key + "," + kvp.Value + "}");
            }
        }
        async Task ISessionEventListener.OnSessionClosed(SessionEventBase sessionEvent)
        {
            Console.WriteLine("Session closed: " + sessionEvent.SessionId);
        }

        async Task ISessionEventListener.OnSessionDataUpdated(SessionEventBase sessionEvent)
        {
            Console.WriteLine("Session data updated");
            Console.WriteLine(sessionEvent.ToString());
            JObject data = sessionEvent.Data as JObject;
            Console.WriteLine(data.ToString());
        }
        async Task ISessionEventListener.OnSessionEventCreated(SessionEventBase sessionEvent)
        {
            Console.WriteLine("Session event created");
            Console.WriteLine(sessionEvent.ToString());
        }

        async Task ISessionEventListener.OnSessionUpdated(SessionEventBase sessionEvent)

        {
            Console.WriteLine("Session Updated Event");
            Console.WriteLine(sessionEvent.ToString());
        }
    }
}

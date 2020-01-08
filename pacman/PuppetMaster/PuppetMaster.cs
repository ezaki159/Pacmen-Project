using System;
using System.Collections.Generic;
using System.Threading;
using CommonInterfaces;

namespace PuppetMaster {
    class PuppetMaster {
        private Dictionary<string, string> _puppetAddresses = new Dictionary<string, string>(); // PID -> Puppet address
        private Dictionary<string, IPuppet> _puppetServices = new Dictionary<string, IPuppet>();
        private string _serverURL;

        public void StartClient(string pid, string pcsURL, string clientURL, int ms, int nPlayers, string filename = null) {
            ProcessCreationService.ProcessCreationService processCreationService = (ProcessCreationService.ProcessCreationService)
                        Activator.GetObject(
                            typeof(ProcessCreationService.ProcessCreationService), pcsURL);
            processCreationService.StartClient(pid, clientURL, _serverURL, ms, nPlayers, filename);

            lock (_puppetAddresses) {
                _puppetAddresses[pid] = clientURL;
            }
        }

        public void StartServer(string pid, string pcsURL, string serverURL, int ms, int nPlayers) {
            _serverURL = serverURL;
            ProcessCreationService.ProcessCreationService processCreationService = (ProcessCreationService.ProcessCreationService)
                Activator.GetObject(
                    typeof(ProcessCreationService.ProcessCreationService), pcsURL);
            processCreationService.StartServer(pid, _serverURL, ms, nPlayers);

            lock (_puppetAddresses) {
                _puppetAddresses[pid] = serverURL;
            }
        }

        public void Wait(int ms) {
            Thread.Sleep(ms);
        }

        public void GlobalStatus() {
            var pids = new List<string>();
            lock (_puppetAddresses) {
                pids.AddRange(_puppetAddresses.Keys);
            }
            foreach (var pid in pids) {
                bool cached;
                lock (_puppetServices) {
                    cached = _puppetServices.ContainsKey(pid);
                }
                if (!cached)
                    GetService(pid);
                lock (_puppetServices) {
                    var service = _puppetServices[pid];
                    new Thread(() => service.GlobalStatus()).Start();
                }
            }
        }

        public void Crash(string pid) {
            bool cached;
            lock (_puppetServices) {
                cached = _puppetServices.ContainsKey(pid);
            }
            if (!cached)
                GetService(pid);

            lock (_puppetServices) {
                try {
                    _puppetServices[pid].Crash();
                }
                catch (Exception) {
                    // It is supposed to throw an Exception, since the service will crash before returning 
                }
            }
        }

        public void Freeze(string pid) {
            bool cached;
            lock (_puppetServices) {
                cached = _puppetServices.ContainsKey(pid);
            }
            if (!cached)
                GetService(pid);

            lock (_puppetServices) {
                _puppetServices[pid].Freeze();
            }
        }

        public void Unfreeze(string pid) {
            bool cached;
            lock (_puppetServices) {
                cached = _puppetServices.ContainsKey(pid);
            }
            if (!cached)
                GetService(pid);

            lock (_puppetServices) {
                _puppetServices[pid].Unfreeze();
            }
        }

        public void InjectDelay(string srcPID, string dstPID) {
            bool cached;
            lock (_puppetServices) {
                cached = _puppetServices.ContainsKey(srcPID);
            }
            if (!cached)
                GetService(srcPID);

            lock (_puppetServices) {
                _puppetServices[srcPID].InjectDelay(dstPID);
            }
        }

        public void LocalState(string pid, int roundID) {
            bool cached;
            lock (_puppetServices) {
                cached = _puppetServices.ContainsKey(pid);
            }
            if (!cached)
                GetService(pid);

            while (true) {
                lock (_puppetServices) {
                    try {
                        Console.WriteLine(_puppetServices[pid].LocalState(roundID));
                        break;
                    }
                    catch (Exception) {
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        private void GetService(string pid) {
            string puppetURL;
            lock (_puppetAddresses) {
                puppetURL = _puppetAddresses[pid];
            }
            IPuppet puppetSlave = (IPuppet) Activator.GetObject(typeof(IPuppet), puppetURL);
            lock (_puppetServices) {
                _puppetServices[pid] = puppetSlave;
            }
        }
    }
}

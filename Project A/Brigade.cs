﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project_A
{
    public class Brigade
    {
        public string Name { get; set; }
        public BrigadeCommander BrigadeCommander { get; set; }
        public List<Worker> Workers { get; set; }
        public string Location;

        public Brigade(string name, BrigadeCommander BrigadeCommandeк, string location) 
        {
            throw new NotImplementedException();
        }

        public void AddWorker(Worker worker) 
        {
            throw new NotImplementedException();
        }

        public bool RemoveWorker(int workerId) 
        {
            throw new NotImplementedException();
        }

        public int GetWorkerCount() 
        {
            throw new NotImplementedException();
        }
    }
}

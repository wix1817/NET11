﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALInterfaces.Models
{
    public class Hero : BaseModel
    {

        public string Name { get; set; }
        public string Races { get; set; }
        public string Subrace { get; set; }
        public string Class { get; set; }
        public string Оrigin { get; set; }
        public int Bone { get; set; }
        public int CreatorId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoUniversityWebApplication.Attribute
{
    [System.AttributeUsage(AttributeTargets.Class |
                           AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)
    ]
    public class Author : System.Attribute
    {
        private string name;
        public double version;

        public Author(string name)
        {
            this.name = name;
            version = 1.0;
        }
    }
}
using Jugnoon.Entity;
using Jugnoon.Framework;
using Jugnoon.Utility;
using System.Collections.Generic;


namespace Jugnoon.Models
{
    public class SearchModelView
    {
        public string SearchTerm { get; set; }

        public string RawTerm { get; set; }

        public bool isSearch { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogEngine6.Models
{
    
     
    public partial class Tag
    {
        public int TagID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Blog_BlogID { get; set; }

        public virtual Blog Blog { get; set; }
    }




}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webionic.LinkList.Models
{
    using Orchard.ContentManagement;

    public class ParentContentItem
    {
        public ContentItem ParentItem { get; set; }
        public IEnumerable<ContentItem> ChildItems { get; set; }

    }
}
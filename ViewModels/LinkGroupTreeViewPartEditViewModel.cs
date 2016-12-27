using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webionic.LinkList.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    using Orchard.Projections.ViewModels;

    public class LinkGroupTreeViewPartEditViewModel
    {
        public IEnumerable<QueryRecordEntry> QueryRecordEntries { get; set; }

        [Required(ErrorMessage = "You must select a Query and a Layout")]
        public int QueryLayoutRecordId { get; set; }

    }
}
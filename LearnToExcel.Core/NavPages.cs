using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LearnToExcel.Core
{
    public static class NavPages
    {
        public static string ActivePageKey => "ActivePage";

        public static string Index => "Index";

        public static string NewAdmissions => "NewAdmissions";

        public static string AdmittedAdmissions => "AdmittedAdmissions";

        public static string PendingAdmissions => "PendingAdmissions";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string NewAdmissionsNavClass(ViewContext viewContext) => PageNavClass(viewContext, NewAdmissions);

        public static string AdmittedAdmissionsNavClass(ViewContext viewContext) => PageNavClass(viewContext, AdmittedAdmissions);

        public static string PendingAdmissionsNavClass(ViewContext viewContext) => PageNavClass(viewContext, PendingAdmissions);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}

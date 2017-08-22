using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PurchaseOrderTracker.Web.Infrastructure
{
    public static class DictionaryExtensions
    {
        public static List<SelectListItem> ToSelectListItem(this IDictionary<int, string> dictionary)
        {
            var selectListItems = new List<SelectListItem>();
            foreach (var entry in dictionary)
                selectListItems.Add(new SelectListItem {Value = entry.Key.ToString(), Text = entry.Value});
            return selectListItems;
        }
    }
}
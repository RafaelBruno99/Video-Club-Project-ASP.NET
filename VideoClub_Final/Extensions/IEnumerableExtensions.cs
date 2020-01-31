using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace VideoClub_Final.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> items, int selectValue)
        {

            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Name"),
                       Value = item.GetPropertyValue("Id"),
                       Selected = item.GetPropertyValue("Id").Equals(selectValue.ToString())
                   };


        }

        public static IEnumerable<SelectListItem> ToSelectListItemString<T>(this IEnumerable<T> items, string selectValue)
        {
            if (selectValue == null)
            {
                selectValue = "";
            }

            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Name"),
                       Value = item.GetPropertyValue("Id"),
                       Selected = item.GetPropertyValue("Id").Equals(selectValue.ToString())
                   };


        }
    }
}

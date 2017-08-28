using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class SiteNote
    {
        private string urlFormat;
        private bool isAutoInc;
        private List<string> menuList;
        private int currentPage;
        private int currentMenu;
        private string nextPageString;

        public string UrlFormat
        {
            get { return urlFormat; }
            set { urlFormat = value; }
        }
        public bool IsAutoInc
        {
            get { return isAutoInc; }
            set { isAutoInc = value; }
        }
        public List<string> MenuList
        {
            get { return menuList; }
            set { menuList = value; }
        }
        public int CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; }
        }
        public int CurrentMenu
        {
            get { return currentMenu; }
            set { currentMenu = value; }
        }
        public string NextPageString
        {
            get { return nextPageString; }
            set { nextPageString = value; }
        }
        private string OnNextPage()
        {
            if (isAutoInc)
            {
                currentPage++;
                return urlFormat.Replace("{menu}", menuList[currentMenu]).Replace("{page}", currentPage.ToString());
            }
            else
            {
                return urlFormat.Replace("{menu}", menuList[currentMenu]).Replace("{page}", nextPageString);
            }
        }
        private string OnNextMenu()
        {
            currentMenu++;
            currentPage = 1;
            nextPageString = "";
            if (isAutoInc)
            {
                currentPage++;
                return urlFormat.Replace("{menu}", menuList[currentMenu]).Replace("{page}", currentPage.ToString());
            }
            else
            {
                return urlFormat.Replace("{menu}", menuList[currentMenu]).Replace("{page}", nextPageString);
            }
        }
        public string OnNext(bool IsNextPage)
        {
            try
            {
                if (IsNextPage) return OnNextPage();
                else return OnNextMenu();
            }
            catch { return ""; }
        }
    }

}

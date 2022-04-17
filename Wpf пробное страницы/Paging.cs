using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Wpf_пробное_страницы
{
    class Paging
    {
        public int PageIndex { get; set; }

        DataTable PagedList = new DataTable();

        public DataTable SetPaging(IList<ThreatList.Threat> ListToPage, int RecordsPerPage)
        {
            int PageGroup = PageIndex * RecordsPerPage;

            IList<ThreatList.Threat> PagedList = new List<ThreatList.Threat>();

            PagedList = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList(); //This is 
                                                                                  //where the Magic Happens. If you have a Specific sort or want to return 
                                                                                  //ONLY a specific set of columns, add it to this LINQ Query.

            DataTable FinalPaging = PagedTable(PagedList);

            string[] ColumnsToBeDeleted = { "Description", "Source", "ImpactObj", "Confidentiality", "Integrity", "Availability" };
            foreach (string ColName in ColumnsToBeDeleted)
            {
                if (FinalPaging.Columns.Contains(ColName))
                    FinalPaging.Columns.Remove(ColName);
            } //показывает только первые две колонны

            return FinalPaging;
        }

        private DataTable PagedTable<T>(IList<T> SourceList)
        {
            Type columnType = typeof(T);
            DataTable TableToReturn = new DataTable();

            foreach (var Column in columnType.GetProperties())
            {
                TableToReturn.Columns.Add(Column.Name, Column.PropertyType);
            }

            foreach (object item in SourceList)
            {
                DataRow ReturnTableRow = TableToReturn.NewRow();
                foreach (var Column in columnType.GetProperties())
                {
                    ReturnTableRow[Column.Name] = Column.GetValue(item);
                }
                TableToReturn.Rows.Add(ReturnTableRow);
            }
            return TableToReturn;
        }

        public DataTable Next(IList<ThreatList.Threat> ListToPage, int RecordsPerPage)
        {
            PageIndex++;
            if (PageIndex >= ListToPage.Count / RecordsPerPage)
            {
                PageIndex = ListToPage.Count / RecordsPerPage;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public DataTable Previous(IList<ThreatList.Threat> ListToPage, int RecordsPerPage)
        {
            PageIndex--;
            if (PageIndex <= 0)
            {
                PageIndex = 0;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public DataTable First(IList<ThreatList.Threat> ListToPage, int RecordsPerPage)
        {
            PageIndex = 0;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public DataTable Last(IList<ThreatList.Threat> ListToPage, int RecordsPerPage)
        {
            PageIndex = ListToPage.Count / RecordsPerPage;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }
    }
}
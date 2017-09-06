using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.BLL
{
    public class PageOfList<T>:List<T>
    {
        public PageInfo Info;
        public PageOfList(IQueryable<T> source,int? pageIndex,int pageSize )
        {
            Info = new PageInfo(pageIndex, pageSize, source.Count());
            AddRange(source.Skip(Info.PageIndex*Info.PageSize).Take(pageSize));
        }
    }
    public class PageInfo
    {
        public int PageIndex { get; private set; }
        public int PageSize {  get; private set; }
        public  int TotalCount {  get; private set; }
        public int PageCount{  get; private set;}

        public PageInfo(int? pageIndex,int pageSize,int totalCount)
        {
            PageIndex = pageIndex ?? 0;
            PageSize = pageSize;
            TotalCount = totalCount;
            PageCount = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}

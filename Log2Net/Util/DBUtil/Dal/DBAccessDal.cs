using Log2Net.Util.DBUtil.Models;

namespace Log2Net.Util.DBUtil.Dal
{
    internal interface IDBAccessDal<T> where T : class
    {
        ExeResEdm GetAll(PageSerach<T> para);

        ExeResEdm Add(AddDBPara<T> dBPara);

    }
}

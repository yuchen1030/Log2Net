using Log2Net.Util.DBUtil.Models;

namespace Log2Net.Util.DBUtil.Dal
{
    internal abstract class DBAccessDal<T> where T : class
    {
        internal abstract ExeResEdm GetAll(PageSerach<T> para);

        internal abstract ExeResEdm Add(AddDBPara<T> dBPara);

    }
}

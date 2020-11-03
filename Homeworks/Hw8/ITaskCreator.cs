using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hw8
{
    public interface ITaskCreator
    {
        public Task<double> CreateTask(Expression expr, Dictionary<Expression, Task<double>> BeforeTasks);
    }
}
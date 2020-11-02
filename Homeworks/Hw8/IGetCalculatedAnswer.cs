using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hw8
{
    public interface IGetCalculatedAnswer
    {
        public Task<double> GetAnswer(Expression<Func<double>> function);
    }
}
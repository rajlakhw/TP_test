using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.flowPlusExternal.ReviewPlus
{
    public class TPTagModel
    {
        public string Text { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Sibling { get; set; }
        public int Length { get; set; }

    }

    /// <summary>
    /// This class implements Equals1 method usefull to implement a custom comparer.
    /// </summary>
    public class TPTagsComparer : IEqualityComparer<TPTagModel>
    {
        /// <summary>
        /// We consider two tags are identical if their text property is the same.
        /// We DON'T compare the start and end position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(TPTagModel x, TPTagModel y)
        {
            return (x.Text == y.Text);
        }

        public int GetHashCode([DisallowNull] TPTagModel obj)
        {
            throw new NotImplementedException();
        }
    }



}

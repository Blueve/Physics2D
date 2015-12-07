using Physics2D.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Common.Events
{
    public delegate void ContactHandle(object sender, ContactEventArgs e);

    public sealed class ContactEventArgs : EventArgs
    {
        #region 公开属性
        public IReadOnlyList<ParticleContact> ContactList { get; }
        #endregion

        #region 构造方法
        public ContactEventArgs(IReadOnlyList<ParticleContact> contact)
        {
            ContactList = contact;
        }
        #endregion
    }
}
